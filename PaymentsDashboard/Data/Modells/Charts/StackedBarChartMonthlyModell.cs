using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Data.Modells.Charts
{
	public class StackedBarChartMonthlyModell
	{
		public DateOnly Month { get; set; }
		public ICollection<TagSum> TagSums { get; set; }
	}
}
