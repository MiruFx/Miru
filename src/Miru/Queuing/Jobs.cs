using System;
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

    public void Enqueue<TJob>(
        TJob job, 
        TimeSpan? startIn = null,
        string queue = "default")
    {
        if (startIn is not null)
        {
            if (job is not INotification)
            {
                App.Framework.Information("Enqueueing Job for {Job}", job);
            
                _backgroundJobClient.Schedule<JobFor<TJob>>(
                    m => m.Execute(job, default, null, queue), 
                    startIn.GetValueOrDefault());
            }
            else
            {
                App.Framework.Information("Enqueueing Notification {Job}", job);
                
                _backgroundJobClient.Schedule<NotificationFor<TJob>>(
                    m => m.Execute(job, default, null), 
                    startIn.GetValueOrDefault());
            }
        }
        else
        {
            if (job is not INotification)
            {
                App.Framework.Information("Enqueueing Job for {Job}", job);
            
                _backgroundJobClient.Enqueue<JobFor<TJob>>(m => m.Execute(job, default, null, queue));
            }
            else
            {
                App.Framework.Information("Enqueueing Notification {Job}", job);
            
                _backgroundJobClient.Enqueue<NotificationFor<TJob>>(m => m.Execute(job, default, null));
            }
        }
    }
}