using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelTennisKrdBot
{
    internal static class DateTimeExtensions
    {
        public static DateTime GetNextDayOfWeek(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            for (int i = 1; ; i++)
            {
                DateTime date = dateTime.AddDays(i);
                if (date.DayOfWeek == dayOfWeek) return date.Date;
            }
        }
    }
}
