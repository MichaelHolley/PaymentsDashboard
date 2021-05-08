using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data.Modells;

namespace PaymentsDashboard.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Payment> Payments { get; set; }
	}
}
