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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Booths/reservation/
        [Route("reservation")]
        [HttpPost]
        public ActionResult<Booth> GetUserReservation([FromBody] string token)
        {
            try
            {
                return Ok(_service.GetUsersBooking(token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
