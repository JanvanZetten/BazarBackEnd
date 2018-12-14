using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Application.Implementation.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResetController : Controller
    {
        private readonly IResetService _service;

        public ResetController(IResetService service)
        {
            _service = service;
        }

        // Put api/reset
        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult ResetAllBoothsAndWaitinglist([FromBody]string token)
        {
            try
            {
                _service.ResetAll(token);
                return Ok("Alle stande og ventelisten er blevet nulstillet.");
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidTokenException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAllowedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Der skete en fejl, prøv igen senere.");
            }
        }
    }
}
