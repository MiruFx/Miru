using Miru.Consolables;

namespace Miru.Queuing;

public class QueueClearConsolable() : 
    Consolable("queue.clear", "Clear all queued jobs")
{
    public class ConsolableHandler(IQueueCleaner queueCleaner) : IConsolableHandler
    {
        public async Task Execute()
        {
            await queueCleaner.ClearAsync();
                
            Console2.GreenLine("Queue has been cleaned");
        }
    }
}