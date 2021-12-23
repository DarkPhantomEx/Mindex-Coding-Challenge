using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController: Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService) 
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        //POST - Creates a Compensation entry
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation comp)
        {
            _logger.LogDebug($"Received compensation creation request for Employee with ID number: '{comp.employee.EmployeeId}'");
            
            _compensationService.Create(comp);

            return CreatedAtRoute("getCompById", new { id = comp.employee.EmployeeId }, comp);
        }

        //GET -Creates a get request for a compensation tied to the employee ID specified
        [HttpGet("employee/{id}", Name = "getCompById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");

            Compensation comp = _compensationService.GetById(id);

            if(comp == null) 
            {
                return NotFound();
            }

            return Ok(comp);
        }

    }
}
