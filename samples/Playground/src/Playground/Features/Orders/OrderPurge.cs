using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Playground.Features.Orders
{
    /// <summary>
    /// Changes orders' status from ForPayment to Cancelled after 1 month
    /// </summary>
    public class OrderPurge
    {
        public class Command : IRequest<Result>
        {
        }

        public class Result
        {
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            public Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Result());
            }
        }
    }
}