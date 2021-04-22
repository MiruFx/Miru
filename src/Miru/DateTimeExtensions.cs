using System;

namespace Miru
{
    public static class DateTimeExtensions
    {
        public static long TwoDigitsYear(this DateTime date)
        {
            return date.ToString("yy").ToLong();
        }
        
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date.Date >= startDate.Date && date.Date <= endDate.Date;
        }
        
        public static DateTime OnlyDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }
        
        public static DateTime Future(this TimeSpan time)
        {
            return DateTime.Now.Add(time);
        }
    }
}