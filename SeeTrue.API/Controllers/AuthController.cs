using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.Infrastructure.Queries;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using SeeTrue.Models;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Healthcheck", Description = "Returns a healthcheck object")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Name = "SeeTrue", Version = 1, Description = "SeeTrue is a user registration and authentication API" });
        }

        [HttpGet("settings")]
        public object Settings()
        {
            throw new NotImplementedException();
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Signup", Description = "Handles email password signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpData data)
        {
            if (SeeTrueConfig.DisableSignup)
            {
                throw new SeeTrueException(HttpStatusCode.Forbidden, "SignUp is disabled");
            }

            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            var user = await this.m.Send(new Infrastructure.Commands.SignUp.Command(data, null));

            return Ok(user);
        }

        [Authorize]
        [HttpPost("invite")]
        public object Invite()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles email change confirmation
        /// </summary>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail([FromBody] Infrastructure.Commands.ConfirmEmailChange.Command data)
        {
            await this.m.Send(data);

            return NoContent();
        }

        /// <summary>
        /// Handles token verfication for signop and recovery
        /// </summary>
        [HttpPost("verify")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Verify(Infrastructure.Commands.Verify.Command data)
        {
            return Ok(await this.m.Send(data with { UserAgent = HttpContext.GetUserAgent() }));
        }

        /// <summary>
        /// Process a magic link request
        /// </summary>
        [HttpPost("magiclink")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MagicLink([FromBody] Infrastructure.Commands.RequestMagicLink.Command data)
        {
            await this.m.Send(data);

            return NoContent();
        }

        /// <summary>
        /// Process a maiclink token
        /// </summary>
        [HttpGet("magiclink")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckMagicLink([FromQuery] string token)
        {
            var result = await this.m.Send(new Infrastructure.Commands.ProcessMagicLink.Command(HttpContext.GetUserAgent(), token));

            return Ok(result);
        }

        /// <summary>
        /// Handlig a recover request
        /// </summary>
        [HttpPost("recover")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Recover([FromBody] Infrastructure.Commands.Recover.Command data)
        {
            await this.m.Send(data);

            return NoContent();
        }

        /// <summary>
        /// Handles login via email and password
        /// Handles Refresh token
        /// </summary>
        [HttpPost("token")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> token([FromBody] TokenData data)
        {
            if (!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest, "Invalid data");
            }

            var result = await this.m.Send(new Infrastructure.Commands.Token.Command(data, null, HttpContext.GetUserAgent()));

            return Ok(result);
        }

        /// <summary>
        /// Returns the current users data
        /// </summary>
        [Authorize]
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.GetUserId();

            return Ok(await this.m.Send(new Infrastructure.Queries.GetUser.Query(userId)));
        }

        /// <summary>
        /// Updates the existing user data
        /// </summary>
        [Authorize]
        [HttpPut("user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] Infrastructure.Commands.UserUpdate.Command data)
        {
            return Ok(await this.m.Send(data));
        }


        /// <summary>
        /// Logs out the users
        /// Revokes all the refresh tokens connected to the given login
        /// Revokes all the access tokens connected to the given login
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
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
