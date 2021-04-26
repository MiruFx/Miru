using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Queuing;
using Miru.Scheduling;
using Quartz;
using Supportreon.Database;
using Supportreon.Features.Donations;

namespace Supportreon.ScheduledTasks
{
    [DisallowConcurrentExecution]
    public class ProcessMonthlyDonationsTask : ScheduledTask
    {
        private readonly Jobs _jobs;
        private readonly SupportreonDbContext _db;

        public ProcessMonthlyDonationsTask(Jobs jobs, SupportreonDbContext db)
        {
            _db = db;
            _jobs = jobs;
        }

        protected override async Task ExecuteAsync()
        {
            var recurrentDonations = await _db.Donations
                .Where(donation => donation.IsRecurrent)
                .ToListAsync();

            recurrentDonations.ForEach(donation =>
            {
                _jobs.PerformLater(new ProcessMonthlyDonationsJob
                {
                    DonationId = donation.Id
                });
            });
        }
    }
}