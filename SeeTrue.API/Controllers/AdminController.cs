using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeeTrue.API.Controllers
{
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator m;

        public AdminController(IMediator m)
        {
            this.m = m;
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetUser.Query(userId)));
        }

        [HttpPut("users/{userId}")]
        public object UpdateUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("users/{userId}")]
        public object DeleterUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        //[Authorize("Admin")]
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromQuery] int page = 1, [FromQuery] int perPgae = 20)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetUsers.Query(page, perPgae)));
        }

        [HttpGet("users")]
        public object ListUsers()
        {
            throw new NotImplementedException();
        }


    }
}
