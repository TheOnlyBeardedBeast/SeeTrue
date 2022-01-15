using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Types;

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
        public async Task<IActionResult> CreateUsers([FromBody] AdminUpdateUserRequest data)
        {
            if (!data.Validate())
            {
                throw new SeeTrueException(System.Net.HttpStatusCode.BadRequest, "Invalid data");
            }

            var user = await this.m.Send(new Infrastructure.Commands.CreateUser.Command
            {
                Email = data.Email,
                Password = data.Password,
                Role = data.Role,
                UserMetaData = data.UserMetaData,
                Language = data.Language,
                Confirm = data.Confirm,
                Audience = data.Audience
            });

            return Ok(user);
        }

        [Authorize("Admin")]
        [HttpPost("authorize")]
        public IActionResult Authorize()
        {
            return NoContent();
        }

        [Authorize("Admin")]
        [HttpGet("mails")]
        public async Task<IActionResult> ListMails([FromQuery] int page = 1, [FromQuery] int perPgae = 20)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetMails.Query(page, perPgae)));
        }

        [Authorize("Admin")]
        [HttpGet("mails/{id}")]
        public async Task<IActionResult> GetMail([FromRoute] Guid id)
        {
            return Ok(await this.m.Send(new Infrastructure.Queries.GetMail.Query(id)));
        }

        [Authorize("Admin")]
        [HttpPost("mails")]
        public async Task<IActionResult> CreateMail([FromBody] Infrastructure.Commands.AdminAddEmail.Command data)
        {
            var result = await this.m.Send(data);

            return Ok(result);
        }

        [Authorize("Admin")]
        [HttpPut("mails")]
        public async Task<IActionResult> UpdateMail([FromBody] Infrastructure.Commands.AdminUpdateEmail.Command data)
        {
            var result = await this.m.Send(data);

            return Ok(result);
        }

        [Authorize("Admin")]
        [HttpGet("settings")]
        public async Task<IActionResult> Settings()
        {
            var result = await this.m.Send(new Infrastructure.Queries.AdminSettings.Query());
            return Ok(result);
        }

    }
}
