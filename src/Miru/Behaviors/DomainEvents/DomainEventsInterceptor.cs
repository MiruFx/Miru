using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;
using Miru.Queuing;

namespace Miru.Behaviors.DomainEvents;

public class DomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;
    private readonly Jobs _jobs;

    public DomainEventsInterceptor(IMediator mediator, Jobs jobs)
    {
        _mediator = mediator;
        _jobs = jobs;
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
        
        foreach (var entity in entitiesSaved)
        {
            while (entity.DomainEvents.TryTake(out IDomainEvent domainEvent))
            {
                await _mediator.Publish(domainEvent, ct);
            }
                
            while (entity.EnqueueEvents.TryTake(out IDomainEvent domainEvent))
            {
                _jobs.PerformLater(domainEvent);
            }
        }
            
        return result;
    }
}