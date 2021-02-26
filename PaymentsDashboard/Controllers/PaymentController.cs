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

		public PaymentController(IPaymentService paymentService)
		{
			this.paymentService = paymentService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<Payment>> GetPayments()
		{
			var payments = paymentService.GetAllPayments().ToList();
			payments.Sort((Payment a, Payment b) => { return b.Date.CompareTo(a.Date); });

			return Ok(payments.RemoveCycle());
		}

		[HttpGet("{numberOfMonths}")]
		public ActionResult<IEnumerable<Payment>> GetPaymentsByMonths(int numberOfMonths)
		{
			List<Payment> payments = paymentService.GetPaymentsByMonths(numberOfMonths).ToList();
			payments.Sort((Payment a, Payment b) => { return b.Date.CompareTo(a.Date); });

			return Ok(payments.RemoveCycle());
		}

		[HttpPost]
		public ActionResult<Payment> CreateOrUpdatePayment(Payment payment)
		{
			if (payment.PaymentId.Equals(Guid.Empty))
			{
				return Ok(paymentService.CreatePayment(payment).RemoveCycle());
			}
			else
			{
				if (paymentService.GetPaymentById(payment.PaymentId) == null)
				{
					return NotFound();
				}

				return Ok(paymentService.UpdatePayment(payment).RemoveCycle());
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

			return Ok(result.RemoveCycle());
		}
	}
}
