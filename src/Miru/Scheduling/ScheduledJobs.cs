using System;
using Hangfire;

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

    private string GetJobId<TJob>(string jobIdSuffix) where TJob : IScheduledJob
    {
        var jobName = typeof(TJob).Name;

        return jobIdSuffix.NotEmpty()
            ? $"{jobName}-{jobIdSuffix}"
            : jobName;
    }
}