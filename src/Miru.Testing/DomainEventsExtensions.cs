using System.Linq;
using Miru.Domain;
using Shouldly;

namespace Miru.Testing
{
    [ShouldlyMethods]
    public static class DomainEventsShouldExtensions
    {
        public static void ShouldPublishEvent<TEvent>(this Entity entity) 
            where TEvent : class, IDomainEvent =>
            entity.DomainEvents
                .OfType<TEvent>()
                .SingleOrDefault()
                .ShouldNotBeNull();
    }
}