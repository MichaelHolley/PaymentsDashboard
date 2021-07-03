using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReoccuringPaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;

		public ReoccuringPaymentController(IPaymentService paymentService)
		{
			this.paymentService = paymentService;
		}

		[HttpGet]
		public ActionResult<IEnumerable<ReoccuringPayment>> GetReoccuringPayments()
		{
			var payments = paymentService.GetAllReoccuringPayments();

			return Ok(payments.RemoveCycle());
		}

		[HttpPost]
		public ActionResult<ReoccuringPayment> CreateOrUpdateReoccuringPayment(ReoccuringPayment payment)
		{
			if (payment.Id.Equals(Guid.Empty))
			{
				return Ok(paymentService.CreateReoccuringPayment(payment).RemoveCycle());
			}
			else
			{
				if (paymentService.GetReoccuringPaymentById(payment.Id) == null)
				{
					return NotFound();
				}

				return Ok(paymentService.UpdateReoccuringPayment(payment).RemoveCycle());
			}
		}

		[HttpDelete("{id}")]
		public ActionResult<ReoccuringPayment> DeleteReoccuringPayment(Guid id)
		{
			var result = paymentService.DeleteReoccuringPaymentById(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result.RemoveCycle());
		}
	}
}
