using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Queuing;

public class NotificationFor<TRequest>
{
    private readonly IMiruApp _app;

    public NotificationFor(IMiruApp app)
    {
        _app = app;
    }

    [DisplayName("{0}")]
    public async Task Execute(TRequest request, CancellationToken ct)
    {
        await _app.ScopedPublishAsync(request, ct);
    }
}