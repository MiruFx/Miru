using Miru.Queuing;

namespace Corpo.Skeleton.Features.Orders;

public class OrderPlaced
{
    // #jobrequest
    public class Request : MiruJob<Request>
    {
        public long OrderId { get; set; }
        public override string Id => OrderId.ToString();
    }
    // #jobrequest
        
    // #jobhandler
    public class Handler : JobHandler<Request>
    {
        public override Task Handle(Request request, CancellationToken ct)
        {
            var orderId = request.OrderId;
                
            // do something

            return Task.CompletedTask;
        }
    }
    // #jobhandler
}