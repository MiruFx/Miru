using System.Linq;
using Microsoft.EntityFrameworkCore;
using Miru.Queuing;
using Miru.Scheduling;
using Quartz;
using Supportreon.Database;
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
        private readonly SupportreonDbContext _db;

        public ProcessMonthlyDonationsTask(Jobs jobs, 
            SupportreonDbContext db)
        {
            _db = db;
            _jobs = jobs;
        }
        protected override async void Execute()
        {
            var recurrentDonations = await _db.Donations
                .Where(donation => donation.IsRecurrent)
                .ToListAsync(default);

            recurrentDonations.ForEach(donation =>
            {
                //New queued job to charge the donation
                _jobs.PerformLater(new ProcessMonthlyDonationsJob()
                {
                    DonationId = donation.Id
                });
            });
        }
    }
}