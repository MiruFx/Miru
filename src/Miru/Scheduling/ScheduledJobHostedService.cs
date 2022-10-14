using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Miru.Scheduling;

public class ScheduledJobHostedService : IHostedService
{
    private readonly IScheduledJobConfig _config;
    private readonly ScheduledJobs _scheduledJobs;

    public ScheduledJobHostedService(
        IScheduledJobConfig config, 
        ScheduledJobs scheduledJobs)
    {
        _config = config;
        _scheduledJobs = scheduledJobs;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _config.Configure(_scheduledJobs);

        _scheduledJobs.DeleteAllObsolete();

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken ct) => await Task.CompletedTask;
}