using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using System;
using System.Linq;

namespace PaymentsDashboard.Services
{
	public class TagService : ITagService
	{
		private readonly DataContext _context;
		public TagService(DataContext context)
		{
			_context = context;
		}

		public Tag CreateTag(Tag tag)
		{
			tag.Payments = null;
			tag.Created = DateTime.Now;

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

		public IQueryable<Tag> GetAllTags(bool includePayments = true)
		{
			if (includePayments)
			{
				return _context.Tags.Include(r => r.Payments);
			}
			else
			{
				return _context.Tags;
			}
		}

		public IQueryable<Tag> GetPrimaryTags()
		{
			return _context.Tags.Include(r => r.Payments).Where(r => r.Type.Equals(TagType.Primary));
		}

		public IQueryable<Tag> GetSecondaryTags()
		{
			return _context.Tags.Include(r => r.Payments).Where(r => r.Type.Equals(TagType.Secondary));
		}

		public Tag GetTagById(Guid Id, bool tracked = false)
		{
			var tag = _context.Tags.Include(t => t.Payments).Where(t => t.TagId.Equals(Id));

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
