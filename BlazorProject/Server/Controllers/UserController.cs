using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorProject.Shared;
using BlazorProject.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("{username}/validate")]
        public UserValidationResult ValidateUser(string username)
        {
            var user = context.Users.FirstOrDefault(x => x.UserName == username);
            return new UserValidationResult
            {
                Exists = user != null
            };
        }
    }
}
