using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.Services
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
			var payment = _context.Payments.Include(p => p.Tags).Where(p => p.PaymentId.Equals(Id));

			if (!tracked)
			{
				payment = payment.AsNoTracking();
			}

			return payment.SingleOrDefault();
		}

		public IQueryable<Payment> GetAllPayments()
		{
			return _context.Payments.Include(r => r.Tags).OrderBy(p => p.Date);
		}


		public IQueryable<Payment> GetPaymentsByMonths(int numberOfMonths)
		{
			DateTime date = DateTime.UtcNow.AddMonths(numberOfMonths * -1);

			return GetAllPayments().Where(r => r.Date.StartsWith(date.Year.ToString()) && r.Date.Contains("-" + date.ToString("MM") + "-"));
		}

		public Payment DeletePaymentById(Guid id)
		{
			var payment = GetPaymentById(id, true);

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
			payment.Tags = GetTrackedTagsList(payment.Tags);

			_context.Payments.Add(payment);
			_context.SaveChanges();

			return payment;
		}

		public Payment UpdatePayment(Payment payment)
		{
			Payment paymentById = GetPaymentById(payment.PaymentId, true);

			paymentById.Amount = payment.Amount;
			paymentById.Date = payment.Date;
			paymentById.Title = payment.Title;
			paymentById.Tags = GetTrackedTagsList(payment.Tags);

			_context.SaveChanges();

			return paymentById;
		}

		private ICollection<Tag> GetTrackedTagsList(IEnumerable<Tag> tags)
		{
			List<Tag> trackedTags = new List<Tag>();
			foreach (var tag in tags)
			{
				trackedTags.Add(_context.Tags.First(t => t.TagId.Equals(tag.TagId)));
			}

			return trackedTags;
		}

		public IQueryable<ReoccuringPayment> GetAllReoccuringPayments()
		{
			return _context.ReoccuringPayments.Include(r => r.Tags).OrderBy(p => p.Created);
		}

		public ReoccuringPayment GetReoccuringPaymentById(Guid Id, bool tracked = false)
		{
			var payment = _context.ReoccuringPayments.Include(p => p.Tags).Where(p => p.Id.Equals(Id));

			if (!tracked)
			{
				payment = payment.AsNoTracking();
			}

			return payment.SingleOrDefault();
		}

		public ReoccuringPayment DeleteReoccuringPaymentById(Guid id)
		{
			var payment = GetReoccuringPaymentById(id, true);

			if (payment == null)
			{
				return null;
			}

			_context.ReoccuringPayments.Remove(payment);
			_context.SaveChanges();

			return payment;
		}

		public ReoccuringPayment CreateReoccuringPayment(ReoccuringPayment payment)
		{
			payment.Tags = GetTrackedTagsList(payment.Tags);

			_context.ReoccuringPayments.Add(payment);
			_context.SaveChanges();

			return payment;
		}

		public ReoccuringPayment UpdateReoccuringPayment(ReoccuringPayment payment)
		{
			ReoccuringPayment paymentById = GetReoccuringPaymentById(payment.Id, true);

			paymentById.Amount = payment.Amount;
			paymentById.Title = payment.Title;
			paymentById.StartDate = payment.StartDate;
			paymentById.EndDate = payment.EndDate;
			paymentById.ReoccuringType = payment.ReoccuringType;
			paymentById.Tags = GetTrackedTagsList(payment.Tags);

			_context.SaveChanges();

			return paymentById;
		}
	}
}
