using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazarRestAPI.DTO;
using Core.Application;
using Core.Application.Implementation;
using Core.Application.Implementation.CustomExceptions;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoothsController : ControllerBase
    {
        private readonly IBoothService _service;
        public BoothsController(IBoothService service)
        {
            _service = service;
        }

        // GET: api/Booths - Get All Booths
        [HttpGet]
        public ActionResult<IEnumerable<Booth>> Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // GET: api/Booths/5 - Get booth with id
        [HttpGet("{id}")]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        //[Authorize]
        [Route("availableCount")] 
        [HttpGet]
        public ActionResult<int> GetAvailableCount()
        {
            try
            {
                return Ok(_service.CountAvailableBooths());
            }
            catch (Exception)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // GET: api/Booths/reservation/
        [Route("reservation")]
        [HttpPost]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // POST: api/Booths - Create booth
        [HttpPost]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // POST: api/Booths/book - Book booth
        [HttpPost]
        [Route("book")]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        [HttpPost]
        [Route("cancelReservation")]
        public ActionResult<Booth> CancelReservation([FromBody] TokenBoothDTO dto)
        {
            try
            {
                return Ok(_service.CancelReservation(dto.id, dto.token));
            }

            catch (BoothNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotAllowedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        [HttpPost]
        [Route("waitinglistPosition")]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // PUT: api/Booths/5 - Update booth
        [HttpPut("{id}")]
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
            catch(Exception)
            {
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        // DELETE: api/ApiWithActions/5 - Delete Booth
        [HttpDelete("{id}")]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }

        [HttpPost]
        [Route("cancelWaitingPosition")]
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
                return BadRequest("Der er sket en fejl. Kontakt din administrator for yderligere information.");
            }
        }
    }
}
