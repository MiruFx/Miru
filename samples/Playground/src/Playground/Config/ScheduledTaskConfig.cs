using Miru.Scheduling;
using Playground.Features.Orders;
using Quartz;

namespace Playground.Config
{
    public class ScheduledTaskConfig : IScheduledTaskConfiguration
    {
        public void Configure(IServiceCollectionQuartzConfigurator config)
        {
            config.ScheduleJob<DispatchSubscriptionOrdersTask>(x => x
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 0)));           
            
            // config.ScheduleJob<OrderPurge>(x => x
            //     .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 0)));
        }
    }
}