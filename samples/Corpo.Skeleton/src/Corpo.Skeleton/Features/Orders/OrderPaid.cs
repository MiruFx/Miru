using Miru.Queuing;

namespace Corpo.Skeleton.Features.Orders;

// #job
public class OrderPaid
{
    public class Request : MiruJob<Request>
    {
    }
        
    public class Handler : JobHandler<Request>
    {
        public override Task Handle(Request request, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}
// #job