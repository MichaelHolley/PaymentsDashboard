using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Data
{
	public class Tag : CreatedBase
	{
		public Guid TagId { get; set; }
		public string Title { get; set; }
		public string HexColorCode { get; set; }
		public ICollection<Payment> Payments { get; set; }
	}
}
