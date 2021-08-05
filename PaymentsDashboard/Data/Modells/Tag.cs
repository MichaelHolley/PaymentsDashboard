using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaymentsDashboard.Data.Modells
{
	public class Tag : EntityBase
	{
		public Guid TagId { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string HexColorCode { get; set; }

		[Required]
		public TagType Type { get; set; }
		public ICollection<Payment> Payments { get; set; }
		public ICollection<ReoccuringPayment> ReoccuringPayments { get; set; }
	}
}
