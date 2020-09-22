using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data
{
	public class PaymentPostModel
	{
		public Guid PaymentId { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public virtual ICollection<Guid> TagIds { get; set; }
	}
}
