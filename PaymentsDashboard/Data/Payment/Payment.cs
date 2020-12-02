using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaymentsDashboard.Data
{
	public class Payment
	{
		public Payment()
		{
			this.Tags = new List<Tag>();
		}

		public Guid PaymentId { get; set; }

		public string Title { get; set; }

		[Range(0, Double.PositiveInfinity)]
		public decimal Amount { get; set; }

		public string Date { get; set; }

		public virtual ICollection<Tag> Tags { get; set; }
	}
}
