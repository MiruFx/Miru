using System.Threading.Tasks;
using Quartz;

namespace Miru.Scheduling 
{
    // public abstract class ScheduledTask : IJob
    // {
    //     protected abstract Task ExecuteAsync();
    //     
    //     public async Task Execute(IJobExecutionContext context)
    //     {
    //         await ExecuteAsync();
    //     }
    // }
    
    public interface IScheduledTask : IJob
    {
        Task ExecuteAsync();

        async Task IJob.Execute(IJobExecutionContext context)
        {
            await ExecuteAsync();
        }
    }
}