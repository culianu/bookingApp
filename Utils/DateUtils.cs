using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookingApp.Utils
{
	public class DateUtils
	{
		public static bool IsPeriodInRange(DateOnly periodStart, DateOnly periodEnd, DateOnly rangeStart, DateOnly rangeEnd)
		{
			if (periodStart > periodEnd)
			{
				//throw new ArgumentException("Period start date cannot be after period end date.");
				Console.WriteLine("Period start date cannot be after period end date.");
			}
			return periodStart >= rangeStart && periodEnd <= rangeEnd;
		}

		public static bool IsPeriodOverlappingWithRange(DateOnly periodStart, DateOnly periodEnd, DateOnly rangeStart, DateOnly rangeEnd)
			{
			if (periodStart > periodEnd)
			{
				//throw new ArgumentException("Period start date cannot be after period end date.");
				Console.WriteLine("Period start date cannot be after period end date.");
			}
			return (rangeStart <= periodEnd && periodEnd <= rangeEnd) || (rangeStart <= periodStart && periodStart <= rangeEnd);
		}
	}
}
