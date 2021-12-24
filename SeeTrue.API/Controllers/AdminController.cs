using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.Infrastructure.Types;

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
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] AdminUpdateUserRequest data)
        {
            var updatedUser = await this.m.Send(new Infrastructure.Commands.AdminUpdateUser.Command(userId, data));

            return Ok(updatedUser);
        }

        [Authorize("Admin")]
        [HttpDelete("users/{userId}")]
        public async Task<NoContentResult> DeleterUser([FromRoute] Guid userId)
        {
            await this.m.Send(new Infrastructure.Commands.DeleteUser.Command(userId));

            return NoContent();
        }

        [Authorize("Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> ListUser([FromQuery] int page = 1, [FromQuery] int perPgae = 20)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetUsers.Query(page, perPgae)));
        }

        [Authorize("Admin")]
        [HttpPost("users")]
        public object CreateUsers([FromBody] AdminUpdateUserRequest data)
        {
            throw new NotImplementedException();
        }


    }
}
