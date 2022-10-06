using System.Threading;
using System.Threading.Tasks;
using Miru.Queuing;
using Miru.Scheduling;
using Playground.Database;

namespace Playground.Features.Orders;

public class DispatchSubscriptionOrdersTask
{
    public class Command : MiruJob<Command>, IScheduledJob
    {
    }

    public class Handler : JobHandler<Command>
    {
        public override async Task Handle(Command request, CancellationToken ct)
        {
            await Task.CompletedTask;
        }
    }
}