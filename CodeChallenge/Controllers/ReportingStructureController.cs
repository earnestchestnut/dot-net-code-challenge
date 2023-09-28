using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<EmployeeController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{employeeId}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult GetEmployeeById(string employeeId)
        {
            string msg = $"Received reporting structure get request for '{employeeId}'";
            _logger.LogDebug(msg);

            var reportingStructure = _reportingStructureService.GetByEmployeeId(employeeId);

            if (reportingStructure == null)
            {
                msg = $"Reporting Structure not found for employeeId {employeeId}";
                _logger.LogDebug(msg);
                return Problem(statusCode: 404, detail: msg, instance: HttpContext.Request.Path);

            }

            return Ok(reportingStructure);
        }
    }
}

