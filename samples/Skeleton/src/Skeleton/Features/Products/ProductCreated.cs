using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Queuing;

namespace Skeleton.Features.Products
{
    public class ProductCreated
    {
        public class Job : IJob
        {
        }

        public class Handler : IRequestHandler<Job>
        {
            public Task<Unit> Handle(Job request, CancellationToken ct)
            {
                return Unit.Task;
            }
        }
    }
}