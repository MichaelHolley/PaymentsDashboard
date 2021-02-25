using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data
{
	public class PaymentViewModel
	{
		public PaymentViewModel(Payment payment)
		{
			this.PaymentId = payment.PaymentId;
			this.Title = payment.Title;
			this.Amount = payment.Amount;
			this.Date = payment.Date;

			this.Tags = new List<Tag>();
			if (payment.Tags != null)
			{
				payment.Tags.ToList().ForEach(tag =>
				{
					tag.Payments = null;
					this.Tags.Add(tag);
				});
			}
		}

		public Guid PaymentId { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public string Date { get; set; }
		public ICollection<Tag> Tags { get; set; }
	}
}
