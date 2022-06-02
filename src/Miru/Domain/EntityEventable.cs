using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miru.Domain;

public abstract class EntityEventable : Entity
{
    [NotMapped]
    private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new();

    [NotMapped]
    public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

    [NotMapped]
    private readonly ConcurrentQueue<Func<IEnqueuedEvent>> _enqueueEvents = new();
    
    [NotMapped]
    public IProducerConsumerCollection<Func<IEnqueuedEvent>> EnqueueEvents => _enqueueEvents;

    protected void PublishEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);

        if (domainEvent is IEnqueuedEvent enqueuedEvent)
        {
            _enqueueEvents.Enqueue(() => enqueuedEvent);
        }
    }
}