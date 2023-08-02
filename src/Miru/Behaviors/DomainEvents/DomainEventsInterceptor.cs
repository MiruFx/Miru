using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;

namespace Miru.Behaviors.DomainEvents;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result,
        CancellationToken ct = default)
    {
        var entitiesSaved = eventData.Context?.ChangeTracker.Entries<EntityEventable>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        if (entitiesSaved == null)
        {
            return result;
        }

        var anyEventPublished = false;
        
        foreach (var entity in entitiesSaved)
        {
            while (entity.DomainEvents.TryTake(out var domainEvent))
            {
                App.Framework.Information("Publishing domain event {Event}", domainEvent);
                
                await _mediator.Publish(domainEvent, ct);

                anyEventPublished = true;
            }
        }

        if (anyEventPublished)
        {
            await eventData.Context?.SaveChangesAsync(ct)!;
        }
        
        return result;
    }
}