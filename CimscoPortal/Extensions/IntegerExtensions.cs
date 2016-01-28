using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimscoPortal.Extensions
{
    public static class IntegerExtensions
    {
        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }

        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }
    }

    public static class DateExtensions
    {
        public static DateTime EndOfTheMonth(this DateTime date)
        {
            var endOfTheMonth = new DateTime(date.Year, date.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            return endOfTheMonth;
        }

        public static DateTime EndOfLastMonth(this DateTime date)
        {
            var endOfLastMonth = new DateTime(date.Year, date.Month, 1)
                .AddDays(-1);

            return endOfLastMonth;
        }
    }
}
