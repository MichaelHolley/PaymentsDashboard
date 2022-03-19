using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Helpers;
using System;
using System.Linq;

namespace PaymentsDashboard.Data
{
	public class DataContext : DbContext
	{
		private readonly IHttpContextAccessor httpContextAccessor;

		public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			this.httpContextAccessor = httpContextAccessor;
		}
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

					((EntityBase)entityEntry.Entity).Owner = httpContextAccessor.HttpContext.GetUserId();
				}
			}

			return base.SaveChanges();
		}
	}
}
