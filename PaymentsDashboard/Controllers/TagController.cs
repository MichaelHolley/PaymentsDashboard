using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentsDashboard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TagController : ControllerBase
	{
		private readonly DataContext _context;

		public TagController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
		{
			return await _context.Tags.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Tag>> GetTag(Guid id)
		{
			var tag = await _context.Tags.FindAsync(id);

			if (tag == null)
			{
				return NotFound();
			}

			return tag;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutTag(Guid id, Tag tag)
		{
			if (id != tag.TagId)
			{
				return BadRequest();
			}

			_context.Entry(tag).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TagExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Tag>> PostTag(Tag tag)
		{
			_context.Tags.Add(tag);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTag", new { id = tag.TagId }, tag);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Tag>> DeleteTag(Guid id)
		{
			var tag = await _context.Tags.FindAsync(id);
			if (tag == null)
			{
				return NotFound();
			}

			_context.Tags.Remove(tag);
			await _context.SaveChangesAsync();

			return tag;
		}

		private bool TagExists(Guid id)
		{
			return _context.Tags.Any(e => e.TagId == id);
		}
	}
}
