using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data
{
	public class Tag
	{
		public Tag()
		{
			this.Payments = new List<Payment>();
		}

		public Guid TagId { get; set; }

		public string Title { get; set; }

		public string HexColorCode { get; set; }

		public virtual ICollection<Payment> Payments { get; set; }
	}
}
