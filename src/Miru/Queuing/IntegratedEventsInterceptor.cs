using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;

namespace Miru.Queuing;

public class IntegratedEventsInterceptor(Jobs jobs) : DbTransactionInterceptor
{
    public override async Task TransactionCommittedAsync(
        DbTransaction transaction,
        TransactionEndEventData eventData,
        CancellationToken cancellationToken = new())
    {
        var entitiesSaved = eventData.Context?.ChangeTracker.Entries<EntityEventable>()
            .Select(po => po.Entity)
            .Where(po => po.EnqueueEvents.Any())
            .ToArray();

        if (entitiesSaved == null)
        {
            return;
        }
        
        foreach (var entity in entitiesSaved)
        {
            while (entity.EnqueueEvents.TryTake(out var enqueuedEvent))
            {
                var job = enqueuedEvent().GetEvent();
                
                App.Framework.Information("Enqueueing {Job} from {Entity}", job, entity);
                
                jobs.Enqueue(enqueuedEvent().GetEvent());
            }
        }
        
        await Task.CompletedTask;
    }
}