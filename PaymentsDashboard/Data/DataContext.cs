using Microsoft.EntityFrameworkCore;

namespace PaymentsDashboard.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Tag> Tags { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public DbSet<PaymentTagRelation> PaymentTagRelations { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
	}
}
