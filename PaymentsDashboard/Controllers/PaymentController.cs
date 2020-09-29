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
	public class PaymentController : ControllerBase
	{
		private readonly DataContext _context;

		public PaymentController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<IEnumerable<PaymentViewModel>> GetPayments()
		{
			List<Payment> payments = _context.Payments.AsNoTracking().Include(r => r.Tags).ThenInclude(r => r.Tag).ToList();

			List<PaymentViewModel> viewList = new List<PaymentViewModel>();
			payments.ForEach(payment => viewList.Add(new PaymentViewModel(payment)));

			viewList.Sort((PaymentViewModel a, PaymentViewModel b) => { return b.Date.CompareTo(a.Date); });

			return viewList;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Payment>> GetPayment(Guid id)
		{
			var payment = await _context.Payments.FindAsync(id);

			if (payment == null)
			{
				return NotFound();
			}

			return payment;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutPayment(Guid id, PaymentPostModel payment)
		{
			if (id != payment.PaymentId)
			{
				return BadRequest();
			}

			_context.Entry(new Payment(payment)).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PaymentExists(id))
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
		public async Task<ActionResult<Payment>> PostPayment(PaymentPostModel payment)
		{
			Payment saved = new Payment(payment);
			_context.Payments.Add(saved);

			List<PaymentTagRelation> relations = new List<PaymentTagRelation>();
			payment.TagIds.ToList<Guid>().ForEach(tagId =>
			{
				var tag = _context.Tags.Find(tagId);

				if (tag == null)
				{
					return;
				}

				relations.Add(
					new PaymentTagRelation()
					{
						Payment = saved,
						Tag = tag
					}
				);
			});

			relations.ForEach(rel =>
			{
				_context.PaymentTagRelations.Add(rel);
			});

			saved.Tags = relations;

			await _context.SaveChangesAsync();

			return CreatedAtAction("GetPayment", new { id = payment.PaymentId }, payment);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Payment>> DeletePayment(Guid id)
		{
			var payment = await _context.Payments.FindAsync(id);
			if (payment == null)
			{
				return NotFound();
			}

			_context.Payments.Remove(payment);
			await _context.SaveChangesAsync();

			return payment;
		}

		private bool PaymentExists(Guid id)
		{
			return _context.Payments.Any(e => e.PaymentId == id);
		}
	}
}
