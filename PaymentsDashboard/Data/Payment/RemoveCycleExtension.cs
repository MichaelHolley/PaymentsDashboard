﻿using System.Collections.Generic;
using System.Linq;

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
					Payments = t.Payments.Select(p =>
						new Payment()
						{
							PaymentId = p.PaymentId,
							Title = p.Title,
							Amount = p.Amount,
							Date = p.Date,
							Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null }).ToList()
						}).ToList()
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
						Tags = p.Tags.Select(pt => new Tag() { TagId = pt.TagId, Title = pt.Title, HexColorCode = pt.HexColorCode, Payments = null }).ToList()
					}
				).ToList();

				return result;
		}
	}
}