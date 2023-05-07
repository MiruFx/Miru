namespace Miru;

public static class DateOnlyExtensions
{
    public static string ToYearMonthDay(this DateOnly dateOnly) => 
        dateOnly.ToString("yyyy-MM-dd");
}