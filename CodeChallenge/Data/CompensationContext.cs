using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
namespace CodeChallenge.Data
{
	public class CompensationContext : DbContext
	{
		public CompensationContext(DbContextOptions<CompensationContext> options) : base(options)
		{

		}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Compensation type is [Keyless]
            // Add key on model create to avoid: Unable to track an instance of type because it does not have a primary key
            modelBuilder.Entity<Compensation>().HasKey(c => new { c.Effectivedate, c.Salary });

        }

        public DbSet<Compensation> Compensations { get; set; }

    }
}