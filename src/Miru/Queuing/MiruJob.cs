using MediatR;

namespace Miru.Queuing;

public abstract class MiruJob<TJob> : IRequest<TJob>, IQueueable
{
    public abstract string Id { get; }

    public override string ToString() => this.Title();
}