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
			payment.Tags.ToList().ForEach(tagRelation => this.Tags.Add(tagRelation.Tag));
		}

		public Guid PaymentId { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public virtual ICollection<Tag> Tags { get; set; }
	}
}
