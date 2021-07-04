using System.Collections.Generic;
using System.Linq;
using PaymentsDashboard.Data.Modells;

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
					p.RemoveCycle();
				}

				foreach (var rp in tag.ReoccuringPayments)
				{
					rp.RemoveCycle();
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
					t.Payments.Clear();
					t.ReoccuringPayments.Clear();
				}
			}

			return payment;
		}

		public static ReoccuringPayment RemoveCycle(this ReoccuringPayment payment)
		{
			if (payment.Tags != null)
			{
				foreach (var t in payment.Tags)
				{
					t.Payments.Clear();
					t.ReoccuringPayments.Clear();
				}
			}

			return payment;
		}

		public static ICollection<Tag> RemoveCycle(this IQueryable<Tag> tags)
		{
			var result = tags.Select(t =>
				new Tag()
				{
					TagId = t.TagId,
					Title = t.Title,
					HexColorCode = t.HexColorCode,
					Type = t.Type,
					Payments = t.Payments.Select(p =>
						new Payment()
						{
							PaymentId = p.PaymentId,
							Title = p.Title,
							Amount = p.Amount,
							Date = p.Date,
							Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null, ReoccuringPayments = null, Type = pt.Type, Created = pt.Created }).ToList(),
							Created = p.Created
						}).ToList(),
					ReoccuringPayments = t.ReoccuringPayments.Select(p =>
						new ReoccuringPayment()
						{
							Id = p.Id,
							Title = p.Title,
							Amount = p.Amount,
							StartDate = p.StartDate,
							EndDate = p.EndDate,
							ReoccuringType = p.ReoccuringType,
							Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null, ReoccuringPayments = null, Type = pt.Type, Created = pt.Created }).ToList(),
							Created = p.Created
						}).ToList(),
					Created = t.Created
				}
			).ToList();

			return result;
		}

		public static ICollection<Payment> RemoveCycle(this IQueryable<Payment> payments)
		{
			var result = payments.Select(p =>
					new Payment()
					{
						PaymentId = p.PaymentId,
						Title = p.Title,
						Amount = p.Amount,
						Date = p.Date,
						Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null, ReoccuringPayments = null, Type = pt.Type, Created = pt.Created }).ToList(),
						Created = p.Created
					}
				).ToList();

				return result;
		}

		public static ICollection<ReoccuringPayment> RemoveCycle(this IQueryable<ReoccuringPayment> payments)
		{
			var result = payments.Select(p =>
					new ReoccuringPayment()
					{
						Id = p.Id,
						Title = p.Title,
						Amount = p.Amount,
						StartDate = p.StartDate,
						EndDate = p.EndDate,
						ReoccuringType = p.ReoccuringType,
						Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null, ReoccuringPayments = null, Type = pt.Type, Created = pt.Created }).ToList(),
						Created = p.Created
					}
				).ToList();

			return result;
		}
	}
}
