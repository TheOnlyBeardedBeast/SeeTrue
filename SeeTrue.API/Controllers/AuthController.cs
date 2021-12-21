using System.Linq;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.Infrastructure.Queries;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Infrastructure.Types;
using Microsoft.AspNetCore.Http;
using System.Net;
using SeeTrue.Models;
using Swashbuckle.AspNetCore.Annotations;
namespace SeeTrue.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator m;

        public AuthController(IMediator m)
        {
            this.m = m;
        }

        [HttpGet("health")]
        [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Healthcheck", Description = "Returns a healthcheck object")]
        public IActionResult HealthCheck()
        {
            return Ok(new HealthCheckResponse("SeeTrue", 1, "SeeTrue is a user registration and authentication API"));
        }

        [HttpGet("settings")]
        [ProducesResponseType(typeof(Infrastructure.Queries.GetSettings.Response), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Instance settings", Description = "Returns publicly available settings for the given instance")]
        public async Task<IActionResult> Settings()
        {
            var settings = await this.m.Send(new Infrastructure.Queries.GetSettings.Query());

            return Ok(settings);
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Signup", Description = "Handles email password signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest data)
        {
            var aud = Request.GetTypedHeaders().Referer.GetLeftPart(UriPartial.Authority);

            if (Env.ValidateAudience)
            {
                var audiences = Helpers.ParseAudiences(Environment.GetEnvironmentVariable("SEETRUE_AUIDIENCES")).ToList();

                if (!audiences.Contains(aud))
                {
                    throw new SeeTrueException(HttpStatusCode.Forbidden, "Not supported client");
                }
            }

            if (Env.SignupDisabled)
            {
                throw new SeeTrueException(HttpStatusCode.Forbidden, "SignUp is disabled");
            }

            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            var user = await this.m.Send(new Infrastructure.Commands.SignUp.Command(data, aud));

            return Ok(user);
        }

        [Authorize]
        [HttpPost("invite")]
        public object Invite()
        {
            throw new NotImplementedException();
        }

        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Email change confirmation", Description = "Handles email change confirmation")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmRequest data)
        {
            await this.m.Send(new Infrastructure.Commands.ConfirmEmailChange.Command(data.Token));

            return NoContent();
        }

        [HttpPost("verify")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Signup | Recovery verification", Description = "Handles token verfication for signup and recovery")]
        public async Task<IActionResult> Verify(VerifyRequest data)
        {
            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            return Ok(await this.m.Send(new Infrastructure.Commands.Verify.Command(data.Type, data.Token, data.Password, HttpContext.GetUserAgent())));
        }

        [HttpPost("magiclink")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Request magic link", Description = "Process a magic link request")]
        public async Task<IActionResult> MagicLink([FromBody] MagicLinkRequest data)
        {
            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            var aud = Request.GetTypedHeaders().Referer.GetLeftPart(UriPartial.Authority);
            await this.m.Send(new Infrastructure.Commands.RequestMagicLink.Command(data.Email, aud));

            return NoContent();
        }

        [HttpGet("magiclink")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Process magic link token", Description = "Processes a magic link token")]
        public async Task<IActionResult> ProcesskMagicLink([FromQuery] string token)
        {
            var result = await this.m.Send(new Infrastructure.Commands.ProcessMagicLink.Command(token, HttpContext.GetUserAgent()));

            return Ok(result);
        }

        [HttpPost("recover")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Recovery request", Description = "Request a recovery token")]
        public async Task<IActionResult> Recover([FromBody] Infrastructure.Commands.Recover.Command data)
        {
            var aud = Request.GetTypedHeaders().Referer.GetLeftPart(UriPartial.Authority);

            await this.m.Send(data with { Audience = aud });

            return NoContent();
        }

        [HttpPost("token")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Login | Refresh", Description = "Handles login via email and password, Handles Refresh token")]
        public async Task<IActionResult> token([FromBody] TokenRequest data)
        {
            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            var aud = Request.GetTypedHeaders().Referer.GetLeftPart(UriPartial.Authority);
            var result = await this.m.Send(new Infrastructure.Commands.Token.Command(data, aud, HttpContext.GetUserAgent()));

            return Ok(result);
        }

        [Authorize]
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "User data", Description = "Returns the current users data")]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.GetUserId();

            return Ok(await this.m.Send(new Infrastructure.Queries.GetUser.Query(userId)));
        }

        [Authorize]
        [HttpPut("user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "User update", Description = "Updates the authentcated users data")]
        public async Task<IActionResult> UpdateUser([FromBody] Infrastructure.Commands.UserUpdate.Command data)
        {
            var userId = HttpContext.GetUserId();

            return Ok(await this.m.Send(data with { UserId = userId }));
        }

        [Authorize]
        [HttpPost("logout")]
        [SwaggerOperation(Summary = "User logout", Description = "Revokes all the refresh tokens connected to the given login, Revokes all the access tokens connected to the given login")]
        public IActionResult Logout()
        {
            var loginId = this.HttpContext.GetLoginId();

            this.m.Send(new Infrastructure.Commands.Logout.Command(loginId));

            return NoContent();
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] string provider)
        {
            var url = await this.m.Send(new Infrastructure.Queries.Authorize.Query(provider));

            return Ok(url);
        }


        [HttpGet("callback")]
        public object Callback([FromQuery] string code, [FromQuery] string state)
        {
            // check if state is in cache, and get the right provider
            // if state doesnt exist in cache abort processing
            // else use code to request POST https://github.com/login/oauth/access_token with header Accept: application/json
            // recieve token
            // request user data
            // GET https://api.github.com/user
            // check if users email in our database if not register him if yes return user
            // issue access and refresh tokens
            // redirect to the frontend with query params

            return Ok(code);
        }
    }
}
