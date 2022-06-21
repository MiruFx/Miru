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

    public void Enqueue<TJob>(TJob job)
    {
        if (job is not INotification)
        {
            App.Framework.Information("Enqueueing Job for {job}", job);
            
            _backgroundJobClient.Enqueue<JobFor<TJob>>(m => m.Execute(job, default, null));
        }
        else
        {
            App.Framework.Information("Enqueueing Notification {job}", job);
            
            _backgroundJobClient.Enqueue<NotificationFor<TJob>>(m => m.Execute(job, default, null));
        }
    }
}