using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Queuing;

namespace Corpo.Skeleton.Features.Orders
{
    public class OrderPlaced
    {
        // #jobrequest
        public class Request : IMiruJob
        {
            public long OrderId { get; set; }
        }
        // #jobrequest
        
        // #jobhandler
        public class Handler : IRequestHandler<Request>
        {
            public async Task<Unit> Handle(Request request, CancellationToken ct)
            {
                var orderId = request.OrderId;
                
                // do something
                
                return await Unit.Task;
            }
        }
        // #jobhandler
    }
}