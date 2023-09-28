using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public long NumberOfReports { get; set; }
    }
}