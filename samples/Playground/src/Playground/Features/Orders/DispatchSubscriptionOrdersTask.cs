using System.Threading.Tasks;
using Miru.Scheduling;
using Playground.Database;

namespace Playground.Features.Orders;

public class DispatchSubscriptionOrdersTask : IScheduledJob, IConsolableTask
{
    private readonly PlaygroundDbContext _db;

    public DispatchSubscriptionOrdersTask(PlaygroundDbContext db)
    {
        _db = db;
    }

    public Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}

public interface IConsolableTask
{
}