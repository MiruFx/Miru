using Microsoft.Extensions.Logging;
using Miru.Queuing;
using Miru.Scheduling;
using Quartz;

namespace Supportreon.Config
{
    public class ScheduledTaskConfiguration : IScheduledTaskConfiguration
    {
        public void Configure(IServiceCollectionQuartzConfigurator configurator)
        {
            configurator.ScheduleJob<ProcessMonthlyDonationsTask>(trigger => trigger
                .WithIdentity("Combined Configuration Trigger")
                .StartNow()
                .WithDailyTimeIntervalSchedule(x => x.WithInterval(10, IntervalUnit.Second))
                .WithDescription("my awesome trigger configured for a job with single call")
            );
        }
    }
    public class ProcessMonthlyDonationsTask : ScheduledTask
    {
        private Jobs _jobs;
        private readonly ILogger<ProcessMonthlyDonationsTask> _logger;

        public ProcessMonthlyDonationsTask(Jobs jobs, ILogger<ProcessMonthlyDonationsTask> logger)
        {
            _jobs = jobs;
            _logger = logger;
        }
        
        protected override void Execute()
        {
            _logger?.LogInformation("Task ProcessMonthlyDonationsTask running....");
        }
    }
}