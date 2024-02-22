using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Hangfire.Storage;
using Miru.Queuing;

namespace Miru.Scheduling;

public class ScheduledJobs
{
    private readonly ScheduledJobOptions _options;
    private readonly JobStorage _jobStorage;
    private readonly Dictionary<string, IScheduledJob> _jobsAdded = new();

    public ScheduledJobs(ScheduledJobOptions options, JobStorage jobStorage)
    {
        _options = options;
        _jobStorage = jobStorage;
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

        // RecurringJob.AddOrUpdate<JobFor<TRequest>>(
        //     jobId,
        //     queueName,
        //     m => m.Execute(request, default, null, queueName),
        //     cron);

        _jobsAdded[jobId] = request;
        
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

    public void DeleteAllObsolete()
    {
        var allJobs = _jobStorage.GetConnection().GetRecurringJobs().ToList();
        
        foreach (var job in allJobs)
            if (_jobsAdded.ContainsKey(job.Id) == false)
                RecurringJob.RemoveIfExists(job.Id);
    }
    
    public IEnumerable<RecurringJobDto> GetAll() =>
        _jobStorage.GetConnection().GetRecurringJobs().ToList();
}