﻿using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;

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
		public ActionResult<IEnumerable<Tag>> GetTags()
		{
			return Ok(tagService.GetAllTags().RemoveCycle());
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<Tag>> GetPrimaryTags()
		{
			return Ok(tagService.GetPrimaryTags().RemoveCycle());
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<Tag>> GetSecondaryTags()
		{
			return Ok(tagService.GetSecondaryTags().RemoveCycle());
		}

		[HttpGet("{id}")]
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
