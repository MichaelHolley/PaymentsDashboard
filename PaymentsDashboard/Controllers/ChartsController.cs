using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells;
using PaymentsDashboard.Data.Modells.Charts;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChartsController : ControllerBase
	{
		private readonly IPaymentService paymentService;
		private readonly ITagService tagService;

		public ChartsController(IPaymentService paymentService, ITagService tagService)
		{
			this.paymentService = paymentService;
			this.tagService = tagService;
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<StackedBarChartMonthlyModell>> GetStackedBarChartByMonths()
		{
			var payments = paymentService.GetAllPayments().ToList();
			var tags = tagService.GetPrimaryTags().RemoveCycle();
			var values = new List<StackedBarChartMonthlyModell>();

			var startDate = new DateOnly(payments.First().Date.Year, payments.First().Date.Month, 1);
			var finalDate = new DateOnly(payments.Last().Date.Year, payments.Last().Date.Month + 1, 1);

			while (startDate < finalDate)
			{
				var temp = new StackedBarChartMonthlyModell()
				{
					Month = startDate,
					TagSums = new List<TagSum>()
				};

				foreach (var tag in tags)
				{
					temp.TagSums.Add(new TagSum()
					{
						Tag = tag,
						Sum = payments.Where(p => p.Tags.Any(t => t.TagId.Equals(tag.TagId)) && p.Date.Month == startDate.Month && p.Date.Year == startDate.Year).Sum(p => p.Amount)
					});
				}

				values.Add(temp);
				startDate = startDate.AddMonths(1);
			}

			return Ok(values);
		}

		[HttpGet("[action]")]
		public ActionResult<IEnumerable<TagSum>> GetMonthlyAverageByTag()
		{
			var payments = paymentService.GetAllPayments().ToList();
			var tags = tagService.GetPrimaryTags().RemoveCycle();
			var values = new List<TagSum>();

			var startDate = new DateOnly(payments.First().Date.Year, payments.First().Date.Month, 1);
			var finalDate = new DateOnly(payments.Last().Date.Year, payments.Last().Date.Month + 1, 1);

			foreach (var tag in tags)
			{
				values.Add(new TagSum() { Tag = tag, Sum = 0 });
			}

			var visitedMonths = new List<DateOnly>();
			foreach (var p in payments)
			{
				var monthOfPayment = new DateOnly(p.Date.Year, p.Date.Month, 1);
				if (!visitedMonths.Any(vm => vm.Equals(monthOfPayment))) {
					visitedMonths.Add(monthOfPayment);
				}

				values.Single(v => v.Tag.TagId.Equals(p.Tags.Single(pT => pT.Type.Equals(TagType.Primary)).TagId)).Sum += p.Amount;
			}

			foreach(var val in values)
			{
				val.Sum /= visitedMonths.Count;
			}

			return Ok(values);
		}
	}
}
