using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TagController : ControllerBase
	{
		private readonly ITagService tagService;

		public TagController(ITagService tagService)
		{
			this.tagService = tagService;
		}

		[HttpGet]
		[Authorize(Policy = "ReadTags")]
		public ActionResult<IEnumerable<Tag>> GetTags()
		{
			return Ok(tagService.GetAllTags().RemoveCycle());
		}

		[HttpGet("[action]")]
		[Authorize(Policy = "ReadTags")]
		public ActionResult<IEnumerable<Tag>> GetPrimaryTags()
		{
			return Ok(tagService.GetPrimaryTags().RemoveCycle());
		}

		[HttpGet("[action]")]
		[Authorize(Policy = "ReadTags")]
		public ActionResult<IEnumerable<Tag>> GetSecondaryTags()
		{
			return Ok(tagService.GetSecondaryTags().RemoveCycle());
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "ReadTags")]
		public ActionResult<Tag> GetTag(Guid id)
		{
			Tag tag = tagService.GetTagById(id);
			if(tag == null)
			{
				return NotFound();
			}

			return Ok(tag.RemoveCycle());
		}

		[HttpPost]
		[Authorize(Policy = "ModifyTags")]
		public ActionResult<Tag> CreateOrUpdateTag(Tag tag)
		{
			if (tag.TagId.Equals(Guid.Empty))
			{
				return Ok(tagService.CreateTag(tag).RemoveCycle());
			}
			else
			{
				if (tagService.GetTagById(tag.TagId) == null)
				{
					return NotFound();
				}

				return Ok(tagService.UpdateTag(tag).RemoveCycle());
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Policy = "ModifyTags")]
		public ActionResult<Tag> DeleteTag(Guid id)
		{
			var result = tagService.DeleteTagById(id);

			if (result == null)
			{
				return NotFound();
			}

			return Ok(result.RemoveCycle());
		}
	}
}
