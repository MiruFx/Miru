using MediatR;

namespace Miru.Queuing;

public static class JobsExtensions
{
    public static void EnqueueIn<TJob>(
        this Jobs jobs, 
        TimeSpan startIn,
        TJob job, 
        string queue = "default") where TJob : IBaseRequest
    {
        jobs.Enqueue(job, startIn: startIn, queue);
    }
}