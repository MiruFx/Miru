using Hangfire;
using Miru.Scheduling;
using Playground.Features.Orders;

namespace Playground.Config;

public class ScheduledJobConfig : IScheduledJobConfig
{
    public void Configure(ScheduledJobs jobs)
    {
        jobs.Add<DispatchSubscriptionOrdersTask>(Cron.Daily(hour: 11));
    }
}