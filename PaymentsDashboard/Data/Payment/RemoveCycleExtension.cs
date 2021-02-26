using System.Collections.Generic;

namespace PaymentsDashboard.Data
{
	public static class RemoveCycleExtension
	{
		public static Tag RemoveCycle(this Tag tag)
		{
			if (tag.Payments != null)
			{
				foreach (var p in tag.Payments)
				{
					foreach (var t in p.Tags)
					{
						t.Payments = null;
					}
				}
			}
			return tag;
		}

		public static Payment RemoveCycle(this Payment payment)
		{
			if (payment.Tags != null)
			{
				foreach (var t in payment.Tags)
				{
					t.Payments = null;
				}
			}

			return payment;
		}

		public static ICollection<Tag> RemoveCycle(this ICollection<Tag> tags)
		{
			foreach (var tag in tags)
			{
				tag.RemoveCycle();
			}

			return tags;
		}

		public static ICollection<Payment> RemoveCycle(this ICollection<Payment> payments)
		{
			foreach (var p in payments)
			{
				p.RemoveCycle();
			}

			return payments;
		}
	}
}
