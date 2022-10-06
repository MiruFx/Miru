using System;
using Hangfire;
using MediatR;
using Miru.Queuing;

namespace Miru.Scheduling;

public class ScheduledJobs
{
    private readonly ScheduledJobOptions _options;

    public ScheduledJobs(ScheduledJobOptions options)
    {
        _options = options;
    }

    public void Add<TJob>(
        string cron, 
        TimeZoneInfo timeZone = null,
        string queueName = null,
        string suffix = null) where TJob : IScheduledJob
    {
        if (timeZone == null)
            timeZone = TimeZoneInfo.Local;

        if (string.IsNullOrEmpty(queueName))
            queueName = _options.QueueName;

        var jobId = GetJobId<TJob>(suffix);
        
        RecurringJob.AddOrUpdate<TJob>(jobId, x => x.ExecuteAsync(), cron, timeZone, queueName);
    }
    
    public void Add<TRequest>(
        TRequest request,
        string cron, 
        TimeZoneInfo timeZone = null,
        string queueName = null,
        string suffix = null) where TRequest : IMiruJob
    {
        if (timeZone == null)
            timeZone = TimeZoneInfo.Local;

        if (string.IsNullOrEmpty(queueName))
            queueName = _options.QueueName;

        var jobId = GetJobId(request, suffix);
        
        RecurringJob.AddOrUpdate<JobFor<TRequest>>(
            jobId, m => m.Execute(request, default, null, queueName), cron, timeZone, queueName);
    }

    private string GetJobId<TRequest>(TRequest request, string jobIdSuffix) 
        where TRequest : IMiruJob
    {
        var jobName = request.GetType().Name;

        return jobIdSuffix.NotEmpty()
            ? $"{jobName}-{jobIdSuffix}"
            : jobName;
    }
    
    private string GetJobId<TJob>(string jobIdSuffix) where TJob : IScheduledJob
    {
        var jobName = typeof(TJob).Name;

        return jobIdSuffix.NotEmpty()
            ? $"{jobName}-{jobIdSuffix}"
            : jobName;
    }
}