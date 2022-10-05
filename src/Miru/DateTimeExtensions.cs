using System;

namespace Miru;

public static class DateTimeExtensions
{
    public static bool Between(this DateTime date, DateTime startDate, DateTime endDate) =>
        date.Date >= startDate.Date && date.Date <= endDate.Date;
        
    public static DateTime Future(this TimeSpan time) => 
        DateTime.Now.Add(time);
        
    public static DateTime OfThisMonth(this int day) => 
        new(DateTime.Now.Year, DateTime.Now.Month, day);
        
    public static DateTime OfNextMonth(this int day) => 
        day.OfThisMonth().AddMonths(1);
        
    public static DateTime OfPreviousMonth(this int day, int howManyMonths = 1) => 
        day.OfThisMonth().AddMonths(-howManyMonths);

    public static DateTime OfMonthsAgo(this int day, int months) => 
        day.OfThisMonth().AddMonths(-months);
    
    public static DateTime OfMonth(this int day, DateTime month) => 
        new(month.Year, month.Month, day);
    
    public static DateTime OfNextMonth(this int day, DateTime month) => 
        day.OfMonth(month).AddMonths(1);
    
    public static DateTime OfMonth(this int day, int year, int month) => 
        new(year, month, day);
}