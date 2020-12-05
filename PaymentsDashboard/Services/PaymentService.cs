using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
			return _context.Payments.Include(r => r.Tags);
		}


		public IQueryable<Payment> GetPaymentsByMonths(int numberOfMonths)
		{
			DateTime date = DateTime.UtcNow.AddMonths(numberOfMonths * -1);

			return _context.Payments.Where(r => r.Date.StartsWith(date.Year.ToString()) && r.Date.Contains("-" + date.ToString("MM") + "-")).Include(r => r.Tags);
		}

		public Payment DeletePaymentById(Guid id)
		{
			var payment = _context.Payments.Find(id);

			if (payment == null)
			{
				return null;
			}

			_context.Payments.Remove(payment);
			_context.SaveChanges();

			return payment;
		}

		public Payment CreatePayment(Payment payment)
		{
			var result = _context.Add(payment);
			_context.SaveChanges();

			return result.Entity;
		}

		public Payment UpdatePayment(Payment payment)
		{
			Payment paymentById = GetPaymentById(payment.PaymentId, true);

			paymentById.Amount = payment.Amount;
			paymentById.Date = payment.Date;
			paymentById.Title = payment.Title;
			paymentById.Tags = payment.Tags;

			_context.SaveChanges();

			return paymentById;
		}
	}
}
