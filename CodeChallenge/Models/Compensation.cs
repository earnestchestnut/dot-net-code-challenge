using Microsoft.EntityFrameworkCore;
using System;

namespace CodeChallenge.Models
{
	[Keyless]
	public class Compensation
    {
		public Employee Employee { get; set; }
		public long Salary { get; set; } = 0;
		public DateTime Effectivedate { get; set; } = DateTime.UtcNow;
    }
}

