using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Miru.Scheduling
{
    public class TaskFactory : IJobFactory 
    {
        private readonly IServiceProvider _serviceProvider;
        
        public TaskFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return ActivatorUtilities.CreateInstance(_serviceProvider, bundle.JobDetail.JobType) as ScheduledTask;
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposableJob)
            {
                disposableJob.Dispose();
            }
        }
    }
}