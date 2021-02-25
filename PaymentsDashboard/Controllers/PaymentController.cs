using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;

		public PaymentController(IPaymentService paymentService, DataContext context)
		{
			this.paymentService = paymentService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Payment>> GetPayments()
		{
			List<Payment> payments = paymentService.GetAllPayments().ToList();
			payments.Sort((Payment a, Payment b) => { return b.Date.CompareTo(a.Date); });

			return Ok(GetPaymentViewModels(payments));
		}

		[HttpGet("{numberOfMonths}")]
		public ActionResult<IEnumerable<Payment>> GetPaymentsByMonths(int numberOfMonths)
		{
			List<Payment> payments = paymentService.GetPaymentsByMonths(numberOfMonths).ToList();
			payments.Sort((Payment a, Payment b) => { return b.Date.CompareTo(a.Date); });

			return Ok(GetPaymentViewModels(payments));
		}

		[HttpPost]
		public ActionResult<Payment> CreateOrUpdatePayment(Payment payment)
		{
			if (payment.PaymentId.Equals(Guid.Empty))
			{
				return Ok(new PaymentViewModel(paymentService.CreatePayment(payment)));
			}
			else
			{
				if (paymentService.GetPaymentById(payment.PaymentId) == null)
				{
					return BadRequest();
				}

				return Ok(new PaymentViewModel(paymentService.UpdatePayment(payment)));
			}
		}

		[HttpDelete("{id}")]
		public ActionResult<Payment> DeletePayment(Guid id)
		{
			var result = paymentService.DeletePaymentById(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(new PaymentViewModel(result));
		}

		private List<PaymentViewModel> GetPaymentViewModels(List<Payment> payments)
		{
			List<PaymentViewModel> viewModels = new List<PaymentViewModel>();
			payments.ForEach(p => viewModels.Add(new PaymentViewModel(p)));

			return viewModels;
		}

	}
}
