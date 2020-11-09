using System;

namespace Miru
{
    public static class TimeSpanExtensions
    {
        public static DateTime Ago(this TimeSpan time)
        {
            return DateTime.Now.Subtract(time);
        }
        
        public static TimeSpan Weeks(this int weeks)
        {
            return new TimeSpan(weeks * 7, 0, 0, 0);
        }
        
        public static TimeSpan Months(this int month)
        {
            return new TimeSpan(month * 30, 0, 0, 0);
        }
    }
}