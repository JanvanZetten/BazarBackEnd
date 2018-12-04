using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Application.Implementation.CustomExceptions;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService UserService)
        {
            _userService = UserService;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userService.GetAll());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                return Ok(_userService.GetByID(id));
            }
            catch(UserNotFoundException e)
            {
                return BadRequest(e);
            }
            catch(Exception e)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public ActionResult<User> Put(int id, [FromBody] User value)
        {
            try
            {
                value.Id = id;
                return Ok(_userService.Update(value));
            }
            catch(NotUniqueUsernameException e)
            {
                return BadRequest(e.Message);
            }
            catch(UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            try
            {
                return Ok(_userService.Delete(id));
            }
            catch (UserNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }
    }
}
