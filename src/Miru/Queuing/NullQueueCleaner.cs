using System.Threading.Tasks;

namespace Miru.Queuing
{
    public class NullQueueCleaner : IQueueCleaner
    {
        public Task Clear()
        {
            return Task.CompletedTask;
        }
    }
}