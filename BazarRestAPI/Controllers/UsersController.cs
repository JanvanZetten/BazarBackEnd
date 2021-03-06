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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private string DefaultExceptionMessage = "Der er sket en fejl. Kontakt din administrator for yderligere information.";

        public UsersController(IUserService UserService, IAuthenticationService AuthService)
        {
            _userService = UserService;
            _authService = AuthService;
        }

        private UserDTO UserToDTO(User user)
        {
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                IsAdmin = user.IsAdmin
            };

            return userDTO;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<User>> Get()
        {
            List<UserDTO> users = new List<UserDTO>();

            foreach (var user in _userService.GetAll())
            {
                users.Add(UserToDTO(user));
            }

            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                User user = _userService.GetByID(id);
                return Ok(UserToDTO(user));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<User> Put(int id, [FromBody] User value)
        {
            try
            {
                value.Id = id;
                value.PasswordHash = null;
                value.PasswordSalt = null;
                User user = _userService.Update(value);
                return Ok(UserToDTO(user));
            }
            catch (NotUniqueUsernameException e)
            {
                return BadRequest(e.Message);
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        [HttpPut("selfupdate")]
        [Authorize]
        public ActionResult<User> UpdateSelfUser([FromBody] TokenUserDTO value)
        {
            try
            {
                if (_authService.VerifyUserFromToken(value.Token) != null)
                {
                    User user = new User()
                    {
                        Id = value.Id,
                        Username = value.Username
                    };

                    user = _userService.Update(user);
                    return Ok(UserToDTO(user));
                }
                return BadRequest(DefaultExceptionMessage);
            }
            catch (InvalidTokenException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotUniqueUsernameException e)
            {
                return BadRequest(e.Message);
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<User> Delete(int id)
        {
            try
            {
                User user = _userService.Delete(id);
                return Ok(UserToDTO(user));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }
    }
}
