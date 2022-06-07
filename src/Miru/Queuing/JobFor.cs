using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Queuing;

public class JobFor<TRequest>
{
    private readonly IMiruApp _app;

    public JobFor(IMiruApp app)
    {
        _app = app;
    }

    [DisplayName("{0}")]
    public async Task Execute(TRequest request, CancellationToken ct)
    {
        await _app.ScopedSendAsync(request as IRequest, ct);
    }
}