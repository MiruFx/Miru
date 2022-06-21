using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Server;
using MediatR;

namespace Miru.Queuing;

public class NotificationFor<TRequest>
{
    private readonly IMiruApp _app;
    private readonly IMediator _mediator;

    public NotificationFor(IMiruApp app, IMediator mediator)
    {
        _app = app;
        _mediator = mediator;
    }

    [DisplayName("{0}")]
    public async Task Execute(TRequest request, CancellationToken ct, PerformContext performContext)
    {
        App.Framework.Information("Publishing Notification {request}", request);
        
        await _mediator.Publish(request, ct);
    }
}