using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;
		private readonly DataContext _context;

		public PaymentController(IPaymentService paymentService, DataContext context)
		{
			this.paymentService = paymentService;
			this._context = context;
		}

		[HttpGet]
		public ActionResult<IEnumerable<PaymentViewModel>> GetPayments()
		{
			List<Payment> payments = paymentService.GetAllPayments().ToList();

			List<PaymentViewModel> viewList = new List<PaymentViewModel>();
			payments.ForEach(payment => viewList.Add(new PaymentViewModel(payment)));

			viewList.Sort((PaymentViewModel a, PaymentViewModel b) => { return b.Date.CompareTo(a.Date); });

			return viewList;
		}

		[HttpGet("{numberOfMonths}")]
		public ActionResult<IEnumerable<PaymentViewModel>> GetPaymentsByMonths(int numberOfMonths)
		{
			List<Payment> payments = paymentService.GetPaymentsByMonths(numberOfMonths).ToList();

			List<PaymentViewModel> viewList = new List<PaymentViewModel>();
			payments.ForEach(payment => viewList.Add(new PaymentViewModel(payment)));

			viewList.Sort((PaymentViewModel a, PaymentViewModel b) => { return b.Date.CompareTo(a.Date); });

			return viewList;
		}

		[HttpPost]
		public async Task<ActionResult<Payment>> CreateOrUpdatePayment(Payment payment)
		{
			return Ok(payment);
		}

		[HttpDelete("{id}")]
		public ActionResult<Payment> DeletePayment(Guid id)
		{
			var result = paymentService.DeletePaymentById(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result);
		}

		private bool PaymentExists(Guid id)
		{
			return _context.Payments.Any(e => e.PaymentId == id);
		}
	}
}
