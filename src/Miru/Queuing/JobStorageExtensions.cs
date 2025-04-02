using System.Linq;
using Hangfire;
using MediatR;

namespace Miru.Queuing;

public static class JobsStorageExtensions
{
    public static void EnqueueIn<TJob>(
        this Jobs jobs, 
        TimeSpan startIn,
        TJob job, 
        string queue = "default") where TJob : IBaseRequest
    {
        jobs.Enqueue(job, startIn: startIn, queue);
    }
    
    public static async Task EnqueueAsync<TJob>(
        this Jobs jobs, 
        TJob job) where TJob : IBaseRequest
    {
        jobs.Enqueue(job);
        
        await Task.CompletedTask;
    }
    
    public static bool AnyEnqueuedFor<TJob>(this JobStorage jobStorage) where TJob : IQueueable =>
        jobStorage
            .GetMonitoringApi()
            .EnqueuedJobs("default", 0, 1000)
            .Select(result => result.Value)
            .Where(enqueueJob => enqueueJob.Job.Args[0].GetType() == typeof(TJob) && enqueueJob.InEnqueuedState)
            .Any();
}