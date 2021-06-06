using System;
using System.Collections.Generic;

namespace PaymentsDashboard.Data.Modells.Statistics
{
	public class StackedBarChartMonthlyModell
	{
		public DateTime Month { get; set; }
		public ICollection<TagSum> TagSums { get; set; }
	}
}
