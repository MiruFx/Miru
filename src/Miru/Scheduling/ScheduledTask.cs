using System;
using System.Threading.Tasks;
using Quartz;

namespace Miru.Scheduling 
{
    public abstract class ScheduledTask : IJob, ISchedulable
    {
        public Task Execute(IJobExecutionContext context)
        {
            Execute();
            
            return Task.CompletedTask;
        }
        public abstract void Execute();

        public void Config(IServiceCollectionQuartzConfigurator config)
        {
            config.ScheduleJob<ScheduledTask>(trigger => 
                trigger
                .WithIdentity("Combined Configuration Trigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .WithDescription("my awesome trigger configured for a job with single call")
            );
        }
    }
}