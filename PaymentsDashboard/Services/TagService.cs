using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Helpers;
using System;
using System.Linq;

namespace PaymentsDashboard.Services
{
	public class TagService : ITagService
	{
		private readonly DataContext _context;
		private readonly IHttpContextAccessor httpContextAccessor;
		public TagService(DataContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			this.httpContextAccessor = httpContextAccessor;
		}

		public Tag CreateTag(Tag tag)
		{
			tag.Payments = null;

			_context.Tags.Add(tag);
			_context.SaveChanges();

			return tag;
		}

		public Tag DeleteTagById(Guid id)
		{
			var tag = GetTagById(id, true);

			if (tag == null)
			{
				return null;
			}

			_context.Tags.Remove(tag);
			_context.SaveChanges();

			return tag;
		}

		public IQueryable<Tag> GetAllTags()
		{
			return _context.Tags.Include(r => r.Payments).Include(r => r.ReoccuringPayments)
				.Where(t => t.Owner.Equals(httpContextAccessor.HttpContext.GetUserId()));
		}

		public IQueryable<Tag> GetPrimaryTags()
		{
			return _context.Tags.Include(r => r.Payments).Include(r => r.ReoccuringPayments)
				.Where(r => r.Type.Equals(TagType.Primary))
				.Where(t => t.Owner.Equals(httpContextAccessor.HttpContext.GetUserId()));
		}

		public IQueryable<Tag> GetSecondaryTags()
		{
			return _context.Tags.Include(r => r.Payments).Include(r => r.ReoccuringPayments)
				.Where(r => r.Type.Equals(TagType.Secondary))
				.Where(t => t.Owner.Equals(httpContextAccessor.HttpContext.GetUserId()));
		}

		public Tag GetTagById(Guid Id, bool tracked = false)
		{
			var tag = _context.Tags.Include(t => t.Payments).Include(r => r.ReoccuringPayments).Where(t => t.TagId.Equals(Id))
				.Where(t => t.Owner.Equals(httpContextAccessor.HttpContext.GetUserId()));

			if (!tracked)
			{
				tag = tag.AsNoTracking();
			}

			return tag.SingleOrDefault();
		}

		public Tag UpdateTag(Tag tag)
		{
			Tag tagById = GetTagById(tag.TagId, true);

			tagById.Title = tag.Title;
			tagById.HexColorCode = tag.HexColorCode;

			_context.SaveChanges();

			return tagById;
		}
	}
}
