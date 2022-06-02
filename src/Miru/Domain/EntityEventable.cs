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
    private readonly ConcurrentQueue<IDomainEvent> _enqueueEvents = new();

    [NotMapped]
    public IProducerConsumerCollection<IDomainEvent> EnqueueEvents => _enqueueEvents;

    protected void PublishEvent(IDomainEvent @event)
    {
        _domainEvents.Enqueue(@event);
    }
    
    protected void EnqueueEvent(IDomainEvent @event)
    {
        _enqueueEvents.Enqueue(@event);
    }
}