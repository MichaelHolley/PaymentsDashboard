using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data
{
	public class PaymentTagRelation
	{
		public Guid Id { get; set; }
		public Payment Payment { get; set; }
		public Tag Tag { get; set; }
	}
}
