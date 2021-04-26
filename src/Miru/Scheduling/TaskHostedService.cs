using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Miru.Scheduling
{
    public class TaskHostedService : IHostedService
    {
        private readonly IScheduler _scheduler;

        public TaskHostedService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken ct)
        {
            return _scheduler.Start(ct);
        }

        public Task StopAsync(CancellationToken ct)
        {
            return _scheduler.Shutdown(ct);
        }
    }
}