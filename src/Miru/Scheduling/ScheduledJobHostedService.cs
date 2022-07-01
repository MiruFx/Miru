using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Miru.Scheduling;

public class ScheduledJobHostedService : IHostedService
{
    private readonly IScheduledJobConfig _config;
    private readonly ScheduledJobs _jobs;

    public ScheduledJobHostedService(
        IScheduledJobConfig config, 
        ScheduledJobs jobs)
    {
        _config = config;
        _jobs = jobs;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        _config.Configure(_jobs);

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken ct) => await Task.CompletedTask;
}