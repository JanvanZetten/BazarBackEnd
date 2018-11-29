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
        private readonly IAuthenticationService _authService;

        public TokensController(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        // POST api/values
        [HttpPost]
        public IActionResult Login([FromBody]  UserDTO userDTO)
        {
            var user = _userService.GetAll().FirstOrDefault(u => u.Username == userDTO.Username);

            if (user == null)
                return Unauthorized();

            if (!_authService.VerifyPaswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            return Ok(new
            {
                username = user.Username,
                token = _authService.GenerateToken(user)
            });
        }

        //[Authorize]
        [Route("createUser")]
        [HttpPost]
        public ActionResult<string> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = new User()
                {
                    Username = userDTO.Username
                };

                var userCreated = _userService.Create(user, userDTO.Password);

                return Ok(_authService.GenerateToken(userCreated));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}