﻿using Microsoft.AspNetCore.Authorization;
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
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			this.paymentService = paymentService;
		}

		[HttpGet]
		[Authorize(Policy = "ReadPayments")]
		public ActionResult<IEnumerable<Payment>> GetPayments()
		{
			var payments = paymentService.GetAllPayments();

			return Ok(payments.RemoveCycle());
		}

		[HttpGet("{numberOfMonths}")]
		[Authorize(Policy = "ReadPayments")]
		public ActionResult<IEnumerable<Payment>> GetPaymentsByMonths(int numberOfMonths)
		{
			var payments = paymentService.GetPaymentsByMonths(numberOfMonths);
			return Ok(payments.RemoveCycle());
		}

		[HttpPost]
		[Authorize(Policy = "ModifyPayments")]
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
		[Authorize(Policy = "ModifyPayments")]
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
