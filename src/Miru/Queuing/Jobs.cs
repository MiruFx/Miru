using System;
using System.Threading;
using Hangfire;

namespace Miru.Queuing
{
    public class Jobs
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public Jobs(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void PerformLater<TJob>(TJob job) where TJob : IMiruJob
        {
            _backgroundJobClient.Enqueue<JobFor<TJob>>(m => m.Execute(job, CancellationToken.None));
        }
    }
}