using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Implementation;
using Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoothsController : ControllerBase
    {
        private readonly BoothService _service;
        public BoothsController(BoothService service)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Booths/5 - Get booth with id
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Booth> Get(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch(Exception ex)
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
                return _service.Create(booth);
            }
            catch(Exception ex)
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
                return _service.Update(booth);
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
                return _service.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
