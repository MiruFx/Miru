using System.Threading.Tasks;
using Miru.Scheduling;
using Playground.Database;

namespace Playground.Features.Orders
{
    public class DispatchSubscriptionOrdersTask : IScheduledTask, IConsolableTask
    {
        private readonly PlaygroundDbContext _db;

        public DispatchSubscriptionOrdersTask(PlaygroundDbContext db)
        {
            _db = db;
        }

        public async Task ExecuteAsync()
        {
        }
    }

    public interface IConsolableTask
    {
    }
}