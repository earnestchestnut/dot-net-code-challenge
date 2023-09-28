using CodeChallenge.Models;

namespace CodeChallenge.Services
{

	public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;


        public ReportingStructureService(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}

		public ReportingStructure GetByEmployeeId(string employee_id)
		{
			Employee employee = _employeeService.GetById(employee_id);
            if (employee is null)
            {
                return null;
            }
            ReportingStructure reportingStructure = new()
            {
                Employee = employee,
                NumberOfReports = GetNumberOfReports(employee)
            };
            ReportingStructure rp = reportingStructure;
			return rp;
 		}

        private static long GetNumberOfReports(Employee employee)
        {
            long numberOfReports = 0;

            if(employee.DirectReports is null)
            {
                return numberOfReports;
            }


            numberOfReports = employee.DirectReports.Count;

            foreach(Employee directReport in employee.DirectReports)
            {
                if(directReport.DirectReports is not null)
                {
                    numberOfReports = numberOfReports + directReport.DirectReports.Count;
                }
            }
            
            return numberOfReports;
        }

	}
}

