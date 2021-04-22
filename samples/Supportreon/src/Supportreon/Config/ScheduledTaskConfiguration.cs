using Microsoft.Extensions.Logging;
using Miru.Queuing;
using Miru.Scheduling;
using Quartz;
using Supportreon.Features.Donations;

namespace Supportreon.Config
{
    public class ScheduledTaskConfiguration : IScheduledTaskConfiguration
    {
        public void Configure(IServiceCollectionQuartzConfigurator configurator)
        {
            configurator.ScheduleJob<ProcessMonthlyDonationsTask>(
                trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(_ => 
                        _.WithIntervalInSeconds(30)
                        .RepeatForever()) 
            );
        }
    }
    
    [DisallowConcurrentExecution]
    public class ProcessMonthlyDonationsTask : ScheduledTask
    {
        private readonly Jobs _jobs;
        private readonly ILogger<ProcessMonthlyDonationsTask> _logger;

        public ProcessMonthlyDonationsTask(Jobs jobs, ILogger<ProcessMonthlyDonationsTask> logger)
        {
            _jobs = jobs;
            _logger = logger;
        }
        protected override void Execute()
        {
            _logger.LogInformation("Task ProcessMonthlyDonationsTask running....");
            _jobs.PerformLater(new DonationRecurringChargeJob());
        }
    }
}