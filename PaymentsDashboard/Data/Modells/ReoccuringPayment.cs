using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Data.Modells
{
	public class ReoccuringPayment : EntityBase
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public ReoccuringType ReoccuringType { get; set; }
		public ICollection<Tag> Tags { get; set; }
	}
}
