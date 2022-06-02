using MediatR;

namespace Miru.Domain;

public interface IEnqueuedEvent
{
    INotification GetJob();
}