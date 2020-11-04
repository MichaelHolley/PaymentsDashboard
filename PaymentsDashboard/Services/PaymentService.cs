using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data.Services
{
	public class PaymentService : IPaymentService
	{

		private readonly DataContext _context;
		public PaymentService(DataContext context)
		{
			_context = context;
		}

		public Payment GetPaymentById(Guid Id, bool tracked = false)
		{
			var payment = _context.Payments.Where(p => p.PaymentId.Equals(Id));

			if (!tracked)
			{
				payment = payment.AsNoTracking();
			}

			return payment.SingleOrDefault();
		}

		public IQueryable<Payment> GetAllPayments()
		{
			return _context.Payments.Include(r => r.Tags).ThenInclude(r => r.Tag);
		}


		public IQueryable<Payment> GetPaymentsByMonths(int numberOfMonths)
		{
			DateTime date = DateTime.UtcNow.AddMonths(numberOfMonths * -1);

			return _context.Payments.Where(r => r.Date.StartsWith(date.Year.ToString()) && r.Date.Contains("-" + date.ToString("MM") + "-"));
		}

		public Payment DeletePaymentById(Guid id)
		{
			var payment = _context.Payments.Find(id);

			if (payment == null)
			{
				return null;
			}

			DeletePaymentTagRelationsByPaymentId(id);

			_context.Payments.Remove(payment);
			_context.SaveChanges();

			return payment;
		}

		public IEnumerable<PaymentTagRelation> DeletePaymentTagRelationsByPaymentId(Guid id)
		{
			var relations = _context.PaymentTagRelations.Where(r => r.Payment.PaymentId.Equals(id));

			if (relations.Count() <= 0)
			{
				return null;
			}

			_context.PaymentTagRelations.RemoveRange(relations);
			_context.SaveChanges();

			return relations;
		}
	}
}
