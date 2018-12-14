using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application;
using Core.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _service;
        private readonly string DefaultExceptionMessage = "Der er sket en fejl. Kontakt din administrator for yderligere information.";

        public LogsController(ILogService service)
        {
            _service = service;
        }

        // GET: api/Logs
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<Log>> Get()
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
    }
}
