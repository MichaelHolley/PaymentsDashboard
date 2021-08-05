using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data.Modells;
using System;
using System.Linq;

namespace PaymentsDashboard.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<ReoccuringPayment> ReoccuringPayments { get; set; }

		public override int SaveChanges()
		{
			var entries = ChangeTracker.Entries().Where(e => 
				e.Entity is EntityBase && (
				e.State == EntityState.Added
				|| e.State == EntityState.Modified));

			foreach (var entityEntry in entries)
			{
				((EntityBase)entityEntry.Entity).Modified = DateTime.Now;

				if (entityEntry.State == EntityState.Added)
				{
					((EntityBase)entityEntry.Entity).Created = DateTime.Now;
				}
			}

			return base.SaveChanges();
		}
	}
}
