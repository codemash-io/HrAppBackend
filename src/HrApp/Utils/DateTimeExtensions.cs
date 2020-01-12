using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrApp
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek lunchDay)
        {
            int diff = (7 + (dt.DayOfWeek - lunchDay)) % 7;
            return dt.AddDays(-1 * diff+7).Date;
        }
    }
}
