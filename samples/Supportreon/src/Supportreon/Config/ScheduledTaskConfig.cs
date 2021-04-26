using Miru.Scheduling;
using Quartz;
using Supportreon.ScheduledTasks;

namespace Supportreon.Config
{
    public class ScheduledTaskConfig : IScheduledTaskConfiguration
    {
        public void Configure(IServiceCollectionQuartzConfigurator config)
        {
            config.ScheduleJob<ProcessMonthlyDonationsTask>(x => x
                .StartNow()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 12, 0)));
        }
    }
}