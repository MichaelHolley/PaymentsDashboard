using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data.Services
{
	public interface IPaymentService
	{
		public IQueryable<Payment> GetAllPayments();
		public Payment GetPaymentById(Guid Id, bool tracked = false);
	}
}
