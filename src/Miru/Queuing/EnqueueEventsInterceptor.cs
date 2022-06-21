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
                var job = enqueuedEvent().GetJob();
                
                App.Framework.Information("Enqueueing {job} from {entity}", job, entity);
                
                _jobs.Enqueue(enqueuedEvent().GetJob());
            }
        }
        
        await Task.CompletedTask;
    }
}