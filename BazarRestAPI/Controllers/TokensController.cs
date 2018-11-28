using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazarRestAPI.DTO;
using Core.Application;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService)
        {
            _userService = userService;
        }
        // POST api/values
        [HttpPost]
        public void Login([FromBody]  UserDTO userDto)
        {
            
        }
        //[Authorize]
        [Route("createUser")]
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody]  User user, UserDTO userDto)
        {
            try
            {
                user.Username = userDto.Username;
                return Ok(_userService.Create(user, userDto.Password));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}