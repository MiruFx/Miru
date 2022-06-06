using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;

namespace Miru.Queuing;

public class EnqueueEventsInterceptor : DbTransactionInterceptor
{
    private readonly Jobs _jobs;

    public EnqueueEventsInterceptor(Jobs jobs)
    {
        _jobs = jobs;
    }

    public override void TransactionCommitted(DbTransaction transaction, TransactionEndEventData eventData)
    {
        base.TransactionCommitted(transaction, eventData);
    }

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
                _jobs.PerformLater(enqueuedEvent().GetJob());
            }
        }
        
        await Task.CompletedTask;
    }
}