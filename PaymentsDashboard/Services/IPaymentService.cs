using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data.Services
{
	public interface IPaymentService
	{
		public IQueryable<Payment> GetAllPayments();
		public IQueryable<Payment> GetPaymentsByMonths(int numberOfMonths);
		public Payment GetPaymentById(Guid Id, bool tracked = false);
		public Payment DeletePaymentById(Guid id);
	}
}