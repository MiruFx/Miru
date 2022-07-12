using MediatR;

namespace Miru.Queuing;

public abstract class MiruNotification : INotification, IQueueable
{
    public abstract string Id { get; }

    public override string ToString() => this.Title();
}