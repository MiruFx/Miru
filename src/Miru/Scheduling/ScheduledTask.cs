using System.Threading.Tasks;
using Quartz;

namespace Miru.Scheduling 
{
    public interface IScheduledTask : IJob
    {
        Task ExecuteAsync();

        async Task IJob.Execute(IJobExecutionContext context)
        {
            await ExecuteAsync();
        }
    }
}