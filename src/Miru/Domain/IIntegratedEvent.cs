using MediatR;

namespace Miru.Domain;

public interface IIntegratedEvent
{
    INotification GetEvent();
}