using System;

namespace Miru.Queuing
{
    public static class MiruCron
    {
        /// <summary>Returns cron expression that fires every second.</summary>
        public static string EverySecond() => "* * * * * *";
        
        /// <summary>Returns cron expression that fires every minute.</summary>
        public static string Minutely() => "* * * * *";

        /// <summary>
        /// Returns cron expression that fires every hour at the first minute.
        /// </summary>
        public static string Hourly() => MiruCron.Hourly(0);

        /// <summary>
        /// Returns cron expression that fires every hour at the specified minute.
        /// </summary>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Hourly(int minute) => $"{(object) minute} * * * *";

        /// <summary>
        /// Returns cron expression that fires every day at 00:00 UTC.
        /// </summary>
        public static string Daily() => MiruCron.Daily(0);

        /// <summary>
        /// Returns cron expression that fires every day at the first minute of
        /// the specified hour in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Daily(int hour) => MiruCron.Daily(hour, 0);

        /// <summary>
        /// Returns cron expression that fires every day at the specified hour and minute
        /// in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Daily(int hour, int minute) => $"{(object) minute} {(object) hour} * * *";

        /// <summary>
        /// Returns cron expression that fires every week at Monday, 00:00 UTC.
        /// </summary>
        public static string Weekly() => MiruCron.Weekly(DayOfWeek.Monday);

        /// <summary>
        /// Returns cron expression that fires every week at 00:00 UTC of the specified
        /// day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        public static string Weekly(DayOfWeek dayOfWeek) => MiruCron.Weekly(dayOfWeek, 0);

        /// <summary>
        /// Returns cron expression that fires every week at the first minute
        /// of the specified day of week and hour in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour) => MiruCron.Weekly(dayOfWeek, hour, 0);

        /// <summary>
        /// Returns cron expression that fires every week at the specified day
        /// of week, hour and minute in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour, int minute) =>
            $"{(object) minute} {(object) hour} * * {(object) (int) dayOfWeek}";

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the first
        /// day of month.
        /// </summary>
        public static string Monthly() => MiruCron.Monthly(1);

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC from today.
        /// </summary>
        public static string MonthlySinceToday() => MiruCron.Monthly(DateTime.Now.Day, 0);

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the specified
        /// day of month.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        public static string Monthly(int day) => MiruCron.Monthly(day, 0);

        /// <summary>
        /// Returns cron expression that fires every month at the first minute of the
        /// specified day of month and hour in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Monthly(int day, int hour) => MiruCron.Monthly(day, hour, 0);

        /// <summary>
        /// Returns cron expression that fires every month at the specified day of month,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Monthly(int day, int hour, int minute) =>
            $"{(object) minute} {(object) hour} {(object) day} * *";

        /// <summary>
        /// Returns cron expression that fires every year on Jan, 1st at 00:00 UTC.
        /// </summary>
        public static string Yearly() => MiruCron.Yearly(1);

        /// <summary>
        /// Returns cron expression that fires every year in the first day at 00:00 UTC
        /// of the specified month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        public static string Yearly(int month) => MiruCron.Yearly(month, 1);

        /// <summary>
        /// Returns cron expression that fires every year at 00:00 UTC of the specified
        /// month and day of month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        public static string Yearly(int month, int day) => MiruCron.Yearly(month, day, 0);
        
        /// <summary>
        /// Returns cron expression that fires every year at 00:00 UTC of the specified
        /// month and day of month.
        /// </summary>
        public static string YearlySinceToday() => MiruCron.Yearly(DateTime.Now.Month, DateTime.Now.Day, 0);

        /// <summary>
        /// Returns cron expression that fires every year at the first minute of the
        /// specified month, day and hour in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Yearly(int month, int day, int hour) => MiruCron.Yearly(month, day, hour, 0);

        /// <summary>
        /// Returns cron expression that fires every year at the specified month, day,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Yearly(int month, int day, int hour, int minute) =>
            $"{(object) minute} {(object) hour} {(object) day} {(object) month} *";

        /// <summary>
        /// Returns cron expression that never fires. Specifically 31st of February
        /// </summary>
        /// <returns></returns>
        public static string Never() => MiruCron.Yearly(2, 31);
    }
}