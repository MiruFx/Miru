using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Miru.Queuing;

public abstract class JobHandler<TJob> : IRequestHandler<TJob, TJob> where TJob : MiruJob<TJob>
{
    public abstract Task Handle(TJob request, CancellationToken ct);

    Task<TJob> IRequestHandler<TJob, TJob>.Handle(TJob request, CancellationToken ct)
    {
        Handle(request, ct);
        return Task.FromResult(request);
    }
}