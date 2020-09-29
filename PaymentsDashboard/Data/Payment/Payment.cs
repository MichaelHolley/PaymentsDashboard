using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaymentsDashboard.Data
{
	public class Payment
	{
		public Payment(PaymentPostModel payment)
		{
			this.PaymentId = payment.PaymentId;
			this.Amount = payment.Amount;
			this.Title = payment.Title;
			this.Date = payment.Date;
			this.Tags = new List<PaymentTagRelation>();
		}

		public Payment() { }

		public Guid PaymentId { get; set; }
		public string Title { get; set; }

		[Range(0, Double.PositiveInfinity)]
		public decimal Amount { get; set; }

		public string Date { get; set; }

		public ICollection<PaymentTagRelation> Tags { get; set; }
	}
}
