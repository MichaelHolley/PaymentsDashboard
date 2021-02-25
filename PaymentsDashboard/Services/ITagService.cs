using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Data.Services
{
	public interface ITagService
	{
		public IQueryable<Tag> GetAllTags();
		public Tag GetTagById(Guid Id, bool tracked = false);
		public Tag DeleteTagById(Guid id);
		public Tag CreateTag(Tag tag);
		public Tag UpdateTag(Tag tag);
	}
}