using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;


        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;

        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] CompensationCreateRequest compensationCreateRequest)
        {
            if(compensationCreateRequest is null)
            {
                string msg = $"Create compensation POST body is null.";
                _logger.LogDebug(msg);
                return Problem(statusCode: 400, detail: msg, instance: HttpContext.Request.Path);


            }

            if (compensationCreateRequest.EmployeeId is null)
            {
                string msg = $"Create compensation POST body is null.";
                _logger.LogDebug(msg);
                return Problem(statusCode: 400, detail: msg, instance: HttpContext.Request.Path);
            }

            Compensation compensation = _compensationService.GetByEmployeeId(compensationCreateRequest.EmployeeId);

            if(compensation is not null)
            {
                string msg = $"Compensation record exists for employee ID {compensationCreateRequest.EmployeeId}.";
                _logger.LogDebug(msg);
                return Problem(statusCode: 409, detail: msg, instance: HttpContext.Request.Path);
            }


            Employee employee = _employeeService.GetById(compensationCreateRequest.EmployeeId);

            if(employee is null)
            {
                string msg = $"Compensation cannot be created. Employee ID {compensationCreateRequest.EmployeeId} was not found.";
                _logger.LogDebug(msg);
                return Problem(statusCode: 400, detail: msg, instance: HttpContext.Request.Path);
            }

            compensation = new()
            {
                Employee = employee,
                Salary = compensationCreateRequest.Salary
            };

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationbyEmployeeId", new { employeeId = compensation.Employee.EmployeeId }, compensation);

        }

        [HttpGet("{employeeId}", Name ="getCompensationbyEmployeeId")]
        public IActionResult GetCompensationByEmplyeeId(string employeeId)
        {
            if(employeeId is null)
            {
                string msg = $"Route param employeeId is null";
                _logger.LogDebug(msg);
                return Problem(statusCode: 422, detail: msg, instance: HttpContext.Request.Path);
            }

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if(compensation is null)
            {
                string msg = $"Compensation not found for employeeId {employeeId}.";
                _logger.LogDebug(msg);
                return Problem(statusCode: 404, detail: msg, instance: HttpContext.Request.Path);
            }

            return Ok(compensation);
        }
    }
}

