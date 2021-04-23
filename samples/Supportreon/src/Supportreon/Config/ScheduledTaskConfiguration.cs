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
            //All scheduled tasks could be registered here. Each one with its own trigger configuration
            configurator.ScheduleJob<ProcessMonthlyDonationsTask>(
                trigger => trigger
                    .StartNow()
                    .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 12, 0))
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
            _jobs.PerformLater(new DonationRecurringChargeMiruJob());
        }
    }
}