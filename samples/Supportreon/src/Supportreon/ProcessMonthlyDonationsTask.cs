using Microsoft.Extensions.Logging;
using Miru.Queuing;
using Miru.Scheduling;

namespace Supportreon
{
    public class ProcessMonthlyDonationsTask : ScheduledTask
    {
        private Jobs _jobs;
        private readonly ILogger<ProcessMonthlyDonationsTask> _logger;

        public ProcessMonthlyDonationsTask(Jobs jobs, ILogger<ProcessMonthlyDonationsTask> logger)
        {
            _jobs = jobs;
            _logger = logger;
        }
        
        public override void Execute()
        {
            _logger.LogInformation("Running task....");
        }
    }
}