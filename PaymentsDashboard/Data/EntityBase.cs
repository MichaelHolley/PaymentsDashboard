using System;

namespace PaymentsDashboard.Data
{
	public abstract class EntityBase
	{
		public DateTime Created { get; set; }
		public DateTime? Modified { get; set; }
	}
}
