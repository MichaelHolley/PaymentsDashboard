using Microsoft.EntityFrameworkCore;

namespace PaymentsDashboard.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Tag> Tags { get; set; }
		public DbSet<Payment> Payments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Payment>().HasMany<Tag>(p => p.Tags).WithMany(t => t.Payments);
		}
	}
}
