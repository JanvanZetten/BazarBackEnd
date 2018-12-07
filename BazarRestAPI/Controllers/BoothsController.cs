using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazarRestAPI.DTO;
using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoothsController : ControllerBase
    {
        private readonly IBoothService _service;
        private string DefaultExceptionMessage = "Der er sket en fejl. Kontakt din administrator for yderligere information.";

        public BoothsController(IBoothService service)
        {
            _service = service;
        }

        // GET: api/Booths - Get All Booths
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<Booth>> Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // GET: api/Booths/IncludeAll - Get All Booths with bookers
        [Route("IncludeAll")]
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<Booth>> GetAllIncludeAll()
        {
            try
            {
                return Ok(_service.GetAllIncludeAll());
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // GET: api/Booths/5 - Get booth with id
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Booth> Get(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }

            catch (BoothNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // GET: api/Booths/availableCount - Get available Booths
        [Route("availableCount")] 
        [HttpGet]
        [Authorize]
        public ActionResult<int> GetAvailableCount()
        {
            try
            {
                return Ok(_service.CountAvailableBooths());
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths/reservation - Get booking
        [Route("reservation")]
        [HttpPost]
        [Authorize]
        public ActionResult<List<Booth>> GetUserReservation([FromBody] string token)
        {
            try
            {
                return Ok(_service.GetUsersBooking(token));
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoBookingsFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths - Create booth
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Booth> Post([FromBody] Booth booth)
        {
            try
            {
                return Ok(_service.Create(booth));
            }
            catch (BoothNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths/book - Book booth
        [Route("book")]
        [HttpPost]
        [Authorize]
        public ActionResult<Booth> BookBooth([FromBody]String token)
        {
            try
            {
                return Ok(_service.Book(token));
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AlreadyOnWaitingListException ex)
            {
                return BadRequest(ex.Message);
            }         
            catch (OnWaitingListException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths/cancelReservation - Removes User from Booth
        [Route("cancelReservation")]
        [HttpPost]
        [Authorize]
        public ActionResult<Booth> CancelReservation([FromBody] TokenBoothDTO dto)
        {
            try
            {
                return Ok(_service.CancelReservation(dto.id, dto.token));
            }

            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotAllowedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths/waitinglistPosition - Gets Waiting List Pos.
        [Route("waitinglistPosition")]
        [HttpPost]
        [Authorize]
        public ActionResult<Booth> WaitingListPosition([FromBody] string token)
        {
            try
            {
                return Ok(_service.GetWaitingListItemPosition(token));
            }

            catch (NotOnWaitingListException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // PUT: api/Booths/5 - Update booth
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Booth> Put(int id, [FromBody] Booth booth)
        {
            try
            {
                booth.Id = id;
                return Ok(_service.Update(booth));
            }
            catch(BoothNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // DELETE: api/Booths/5 - Delete Booth
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<Booth> Delete(int id)
        {
            try
            {
                return Ok(_service.Delete(id));
            }
            catch(BoothNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/Booths/cancelWaitingPosition - Remove user from Waiting List
        [Route("cancelWaitingPosition")]
        [HttpPost]
        [Authorize]
        public ActionResult<WaitingListItem> CancelWaitingPosition([FromBody] TokenBoothDTO dto)
        {
            try
            {
                return Ok(_service.CancelWaitingPosition(dto.token));
            }
            catch (WaitingListItemNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotAllowedException ex)
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
