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

        [Authorize("Admin")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetUser.Query(userId)));
        }

        [Authorize("Admin")]
        [HttpPut("users/{userId}")]
        public object UpdateUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        [HttpDelete("users/{userId}")]
        public object DeleterUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        [Authorize("Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> ListUser([FromQuery] int page = 1, [FromQuery] int perPgae = 20)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetUsers.Query(page, perPgae)));
        }

        [Authorize("Admin")]
        [HttpPost("users")]
        public object CreateUsers()
        {
            throw new NotImplementedException();
        }


    }
}
