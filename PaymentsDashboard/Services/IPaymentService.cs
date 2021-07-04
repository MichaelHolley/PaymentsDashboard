using System;
using System.Linq;
using PaymentsDashboard.Data.Modells;

namespace PaymentsDashboard.Services
{
	public interface IPaymentService
	{
		public IQueryable<Payment> GetAllPayments();
		public IQueryable<Payment> GetPaymentsByMonths(int numberOfMonths);
		public Payment GetPaymentById(Guid Id, bool tracked = false);
		public Payment DeletePaymentById(Guid id);
		public Payment CreatePayment(Payment payment);
		public Payment UpdatePayment(Payment payment);
		public IQueryable<ReoccuringPayment> GetAllReoccuringPayments();
		public ReoccuringPayment GetReoccuringPaymentById(Guid Id, bool tracked = false);
		public ReoccuringPayment DeleteReoccuringPaymentById(Guid id);
		public ReoccuringPayment CreateReoccuringPayment(ReoccuringPayment payment);
		public ReoccuringPayment UpdateReoccuringPayment(ReoccuringPayment payment);

	}
}