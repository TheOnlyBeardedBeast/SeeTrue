using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.Utils.Extensions;
using SeeTrue.Utils.Types;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeeTrue.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class AuthController : ControllerBase
    {
        protected readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
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
        public async Task<object> SignUp([FromBody] SignUpData data)
        {
            return await mediator.Send(new CQRS.Commands.SignUp.Command(data, null));
        }

        [HttpPost("invite")]
        public object Invite()
        {
            throw new NotImplementedException();
        }

        [HttpPost("verify")]
        public object Verify()
        {
            throw new NotImplementedException();
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
        public object token([FromBody] TokenData data)
        {
            return data.Validate();
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
