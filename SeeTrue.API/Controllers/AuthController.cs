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

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] Infrastructure.Commands.ConfirmEmailChange.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [HttpPost("verify")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Verify(Infrastructure.Commands.Verify.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [HttpPost("magiclink")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MagicLink([FromBody] Infrastructure.Commands.RequestMagicLink.Command data)
        {
            await this.m.Send(data);

            return NoContent();
        }

        [HttpGet("magiclink")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserTokenResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckMagicLink([FromQuery] string token)
        {
            var result = await this.m.Send(new Infrastructure.Commands.ProcessMagicLink.Command(token));

            return Ok(result);
        }

        [HttpPost("recover")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Recover([FromBody] Infrastructure.Commands.Recover.Command data)
        {
            await this.m.Send(data);

            return NoContent();
        }

        [HttpPost("token")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> token([FromBody] TokenData data)
        {
            if(!data.Validate())
            {
                throw new SeeTrueException(HttpStatusCode.BadRequest,"Invalid data");
            }

            var result = await this.m.Send(new Infrastructure.Commands.Token.Command(data, null));

            return Ok(result);
        }

        [Authorize]
        [HttpGet("user")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.GetUserId();

            return Ok(await this.m.Send(new Infrastructure.Queries.GetUser.Query(userId)));
        }

        [Authorize]
        [HttpPut("user")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] Infrastructure.Commands.UserUpdate.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [Authorize]
        [HttpPost("logout")]
        public object Logout()
        {
            throw new NotImplementedException();
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] string provider)
        {
            var url = this.m.Send(new Infrastructure.Queries.Authorize.Query(provider));

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
