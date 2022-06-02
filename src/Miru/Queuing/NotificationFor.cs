using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Queuing;

public class NotificationFor<TRequest>
{
    private readonly IMediator _mediator;

    public NotificationFor(IMediator mediator)
    {
        _mediator = mediator;
    }

    [DisplayName("{0}")]
    public async Task Execute(TRequest request, CancellationToken ct)
    {
        await _mediator.Publish(request, ct);
    }
}