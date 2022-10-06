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

    public string Add<TRequest>(
        TRequest request,
        string cron, 
        TimeZoneInfo timeZone = null,
        string queueName = null,
        string suffix = null) where TRequest : IScheduledJob
    {
        if (timeZone == null)
            timeZone = TimeZoneInfo.Local;

        if (string.IsNullOrEmpty(queueName))
            queueName = _options.QueueName;

        var jobId = GetJobId(request, suffix);
        
        RecurringJob.AddOrUpdate<JobFor<TRequest>>(
            jobId, m => m.Execute(request, default, null, queueName), cron, timeZone, queueName);

        return jobId;
    }

    private string GetJobId<TRequest>(TRequest request, string jobIdSuffix) 
        where TRequest : IScheduledJob
    {
        var featureInfo = new FeatureInfo(request);
        
        var jobName = featureInfo.GetName();

        return jobIdSuffix.NotEmpty()
            ? $"{jobName}-{jobIdSuffix}"
            : jobName;
    }
}