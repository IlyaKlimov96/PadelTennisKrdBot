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
                DateTime date = DateTime.Now.AddDays(i);
                if (date.DayOfWeek == dayOfWeek) return date.Date;
            }
        }

        public static string ToStringRu(this DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Чертверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => string.Empty
            };
        }
    }
}
