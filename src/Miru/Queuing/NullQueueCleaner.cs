using System.Threading.Tasks;

namespace Miru.Queuing
{
    public class NullQueueCleaner : IQueueCleaner
    {
        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }
    }
}