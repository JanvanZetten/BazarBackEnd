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
        // GET: api/Booths
        [HttpGet]
        public ActionResult<IEnumerable<Booth>> GetBooths()
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

        // GET: api/Booths/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Booths
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Booths/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
