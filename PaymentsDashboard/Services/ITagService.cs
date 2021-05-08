using System;
using System.Linq;
using PaymentsDashboard.Data.Modells;

namespace PaymentsDashboard.Services
{
	public interface ITagService
	{
		public IQueryable<Tag> GetAllTags();
		public IQueryable<Tag> GetPrimaryTags();
		public IQueryable<Tag> GetSecondaryTags();
		public Tag GetTagById(Guid Id, bool tracked = false);
		public Tag DeleteTagById(Guid id);
		public Tag CreateTag(Tag tag);
		public Tag UpdateTag(Tag tag);
	}
}