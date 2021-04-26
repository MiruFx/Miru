using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Queuing;

namespace Corpo.Skeleton.Features.Orders
{
    // #job
    public class OrderPaid
    {
        public class Request : IMiruJob
        {
        }
        
        public class Handler : IRequestHandler<Request>
        {
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                return Unit.Task;
            }
        }
    }
    // #job
}