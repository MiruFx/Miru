using Miru.Domain;

namespace Miru.Testing;

[ShouldlyMethods]
public static class DomainEventsShouldExtensions
{
    public static void ShouldPublishEvent<TEvent>(this EntityEventable entity) 
        where TEvent : class, IDomainEvent =>
            entity.DomainEvents
                .OfType<TEvent>()
                .SingleOrDefault()
                .ShouldNotBeNull($"Could not find event of type {typeof(TEvent).FullName}");
}