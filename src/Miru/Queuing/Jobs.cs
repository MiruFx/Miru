using System;
using System.Threading;
using Hangfire;

namespace Miru.Queuing
{
    public class Jobs
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public Jobs(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        public void PerformLater<TJob>(TJob job) where TJob : IJob
        {
            _backgroundJobClient.Enqueue<JobFor<TJob>>(m => m.Execute(job, CancellationToken.None));
        }

        public string PerformLater<TJob>(TJob job, string cronExpression) where TJob : IJob
        {
            var jobId = Guid.NewGuid().ToString();
            
            _recurringJobManager.AddOrUpdate<JobFor<TJob>>(
                jobId,
                m => m.Execute(job, CancellationToken.None),
                cronExpression, 
                TimeZoneInfo.Local);
            
            return jobId;
        }
        
        public void RemoveIfExists(string jobId)
        {
            _recurringJobManager.RemoveIfExists(jobId);
        }
    }
}