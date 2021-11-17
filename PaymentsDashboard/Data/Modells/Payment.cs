using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaymentsDashboard.Data.Modells
{
	public class Payment : EntityBase
	{
		public Guid PaymentId { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }

		[Required]
		public DateOnly Date { get; set; }
		public ICollection<Tag> Tags { get; set; }
	}
}
