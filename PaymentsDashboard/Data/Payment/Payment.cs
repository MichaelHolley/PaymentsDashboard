using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Data
{
	public class Payment
	{
		public Guid PaymentId { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public string Date { get; set; }
		public ICollection<Tag> Tags { get; set; }
	}
}
