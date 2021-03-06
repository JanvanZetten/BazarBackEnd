﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazarRestAPI.DTO;
using Core.Application;
using Core.Application.Implementation.CustomExceptions;
using Core.Entity;
using Microsoft.AspNetCore.Authorization;
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
        private string DefaultExceptionMessage = "Der er sket en fejl. Kontakt din administrator for yderligere information.";

        public TokensController(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Login([FromBody]  UserDTO userDTO)
        {
            var user = _userService.GetAll().FirstOrDefault(u => u.Username.ToLower() == userDTO.Username.ToLower());

            var wrong = BadRequest("Wrong username or password.");
            wrong.StatusCode = 401;

            if (user == null)
                return wrong;

            if (!_authService.VerifyPaswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                return wrong;

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
                    Username = userDTO.Username,
                    IsAdmin = false
                };

                var userCreated = _userService.Create(user, userDTO.Password);

                return Ok(new { username = userDTO.Username});
            }
            catch(InputNotValidException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotUniqueUsernameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }

        }

        [Route("createUserAdmin")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<string> CreateUserAdmin([FromBody] UserDTO userDTO)
        {
            try
            {
                var user = new User()
                {
                    Username = userDTO.Username,
                    IsAdmin = userDTO.IsAdmin
                };

                var userCreated = _userService.Create(user, userDTO.Password);

                return Ok(new { username = userDTO.Username });
            }
            catch (InputNotValidException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotUniqueUsernameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }

        }
    }
}