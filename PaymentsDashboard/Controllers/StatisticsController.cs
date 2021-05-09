using Microsoft.AspNetCore.Mvc;
using PaymentsDashboard.Data;
using PaymentsDashboard.Data.Modells.Statistics;
using PaymentsDashboard.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentsDashboard.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StatisticsController : ControllerBase
	{
		private readonly IPaymentService paymentService;
		private readonly ITagService tagService;

		public StatisticsController(IPaymentService paymentService, ITagService tagService)
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

			var startDate = new DateTime(DateTime.Parse(payments.First().Date).Year, DateTime.Parse(payments.First().Date).Month, 1);
			var finalDate = new DateTime(DateTime.Parse(payments.Last().Date).Year, DateTime.Parse(payments.Last().Date).Month + 1, 1);

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
						Sum = payments.Where(p => p.Tags.Any(t => t.TagId.Equals(tag.TagId)) && DateTime.Parse(p.Date).Month == startDate.Month && DateTime.Parse(p.Date).Year == startDate.Year).Sum(p => p.Amount)
					});
				}

				values.Add(temp);
				startDate = startDate.AddMonths(1);
			}

			return Ok(values);
		}
	}
}
