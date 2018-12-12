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
    public class ImageUrlController : ControllerBase
    {
        private string DefaultExceptionMessage = "Der er sket en fejl. Kontakt din administrator for yderligere information.";
        private readonly IImageURLService _service;

        public ImageUrlController(IImageURLService service)
        {
            this._service = service;
        }

        // GET: api/ImageUrl
        [HttpGet]
        public ActionResult<List<ImageURL>> Get()
        {
            try
            { 
            return Ok(_service.GetAll());
            }
            catch(Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // GET: api/ImageUrl/5
        [HttpGet("{id}")]
        public ActionResult<ImageURL> Get(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (ImageURLNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // POST: api/ImageUrl
        [HttpPost]
        public ActionResult<ImageURL> Post([FromBody] ImageURL imageUrl)
        {
            try
            {
                return Ok(_service.Create(imageUrl));
            }
            catch(InputNotValidException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IncompatibleFileTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // PUT: api/ImageUrl/5
        [HttpPut("{id}")]
        public ActionResult<ImageURL> Put(int id, [FromBody] ImageURL imageURL)
        {
            try
            {
                imageURL.Id = id;
                return Ok(_service.Update(imageURL));
            }
            catch(ImageURLNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InputNotValidException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (IncompatibleFileTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest(DefaultExceptionMessage);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult<ImageURL> Delete(int id)
        {
            try
            {
                return Ok(_service.Delete(id));
            }
            catch (ImageURLNotFoundException ex)
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
