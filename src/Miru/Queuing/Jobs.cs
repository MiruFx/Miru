using Hangfire;
using MediatR;

namespace Miru.Queuing;

public class Jobs
{
    private readonly IBackgroundJobClient _backgroundJobClient;

    public Jobs(IBackgroundJobClient backgroundJobClient)
    {
        _backgroundJobClient = backgroundJobClient;
    }

    public void PerformLater<TJob>(TJob job)
    {
        if (job is not INotification)
        {
            _backgroundJobClient.Enqueue<JobFor<TJob>>(m => m.Execute(job, default));
        }
        else
        {
            _backgroundJobClient.Enqueue<NotificationFor<TJob>>(m => m.Execute(job, default));
        }
    }
}