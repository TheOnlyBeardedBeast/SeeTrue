using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
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

        [HttpPost("invite")]
        public object Invite()
        {
            throw new NotImplementedException();
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
        public object Recover()
        {
            throw new NotImplementedException();
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

        [HttpGet("user")]
        public object GetUser()
        {
            throw new NotImplementedException();
        }

        [HttpPut("user")]
        public object UpdateUser()
        {
            throw new NotImplementedException();
        }

        [HttpPost("logout")]
        public object Logout()
        {
            throw new NotImplementedException();
        }

        [HttpGet("authorize")]
        public object Authorize()
        {
            throw new NotImplementedException();
        }

        [HttpPut("callback")]
        public object Callback()
        {
            throw new NotImplementedException();
        }
    }
}
