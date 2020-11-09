using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Queuing
{
    public class JobFor<TRequest>
    {
        private readonly IMediator _mediator;

        public JobFor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(TRequest request, CancellationToken ct)
        {
            await _mediator.Send(request, ct);
        }
    }
}