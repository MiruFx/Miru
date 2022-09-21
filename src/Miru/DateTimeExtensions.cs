using System;

namespace Miru;

public static class DateTimeExtensions
{
    public static long TwoDigitsYear(this DateTime date) => date.ToString("yy").ToLong();
        
    public static bool Between(this DateTime date, DateTime startDate, DateTime endDate) =>
        date.Date >= startDate.Date && date.Date <= endDate.Date;
        
    public static DateTime OnlyDate(this DateTime date) => new DateTime(date.Year, date.Month, date.Day);
        
    public static DateTime Future(this TimeSpan time) => DateTime.Now.Add(time);
        
    public static DateTime OfThisMonth(this int day) => new(DateTime.Now.Year, DateTime.Now.Month, day);
        
    public static DateTime OfNextMonth(this int day) => day.OfThisMonth().AddMonths(1);
        
    public static DateTime OfPreviousMonth(this int day) => day.OfThisMonth().AddMonths(-1);
    
    public static DateTime OfMonthsAgo(this int day, int months) => day.OfThisMonth().AddMonths(-months);
    
    public static DateTime OfMonth(this int day, DateTime month) => new(month.Year, month.Month, day);
}