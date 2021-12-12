using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.CQRS.Commands;
using SeeTrue.Utils;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Types;

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
                return Forbid("SignUp is disabled");
            }

            if (!data.Validate())
            {
                return BadRequest("Ivalid data.");
            }

            try
            {
                var user = await this.m.Send(new CQRS.Commands.SignUp.Command(data, null));
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("invite")]
        public object Invite()
        {
            throw new NotImplementedException();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] CQRS.Commands.ConfirmEmailChange.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(CQRS.Commands.Verify.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [HttpGet("verify")]
        public object CheckVerify()
        {
            throw new NotImplementedException();
        }

        [HttpPost("magiclink")]
        public object MagicLink()
        {
            throw new NotImplementedException();
        }

        [HttpPost("recover")]
        public async Task<IActionResult> Recover([FromBody] CQRS.Commands.Recover.Command data)
        {
            await this.m.Send(data);
            return Ok();
        }

        [HttpPost("token")]
        public async Task<IActionResult> token([FromBody] TokenData data)
        {
            if(!data.Validate())
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var result = await this.m.Send(new CQRS.Commands.Token.Command(data, null));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.GetUserId();

            return Ok(await this.m.Send(new CQRS.Queries.GetUser.Query(userId)));
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] CQRS.Commands.UserUpdate.Command data)
        {
            return Ok(await this.m.Send(data));
        }

        [HttpPost("logout")]
        public object Logout()
        {
            throw new NotImplementedException();
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize([FromQuery] string provider)
        {
            var url = this.m.Send(new CQRS.Queries.Authorize.Query(provider));

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

            return Ok(code);
        }
    }
}
