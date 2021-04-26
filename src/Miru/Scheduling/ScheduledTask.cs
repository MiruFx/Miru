using System.Threading.Tasks;
using Quartz;

namespace Miru.Scheduling 
{
    public abstract class ScheduledTask : IJob
    {
        protected abstract void Execute();
        public Task Execute(IJobExecutionContext context)
        {
            Execute();
            return Task.CompletedTask;
        }
    }
}