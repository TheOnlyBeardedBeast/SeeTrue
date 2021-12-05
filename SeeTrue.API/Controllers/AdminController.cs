using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeeTrue.API.Controllers
{
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("users/{userId}")]
        public object GetUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
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

        [HttpPost("users")]
        public object CreateUser()
        {
            throw new NotImplementedException();
        }

        [HttpGet("users")]
        public object ListUsers()
        {
            throw new NotImplementedException();
        }

        
    }
}
