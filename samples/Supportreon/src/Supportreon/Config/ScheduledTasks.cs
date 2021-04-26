using System;
using Quartz;

namespace Supportreon.Config
{
    public class ScheduledTasks
    {

        public void Config<TTask>(IServiceCollectionQuartzConfigurator config) where TTask : IJob
        {
            config.ScheduleJob<TTask>(trigger => trigger
                .WithIdentity("Combined Configuration Trigger")
                .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
                .WithDescription("my awesome trigger configured for a job with single call"));
        }
        
    }
}