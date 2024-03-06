using System.Threading;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Domain;

namespace Miru.Testing;

public class EntitySavedInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, 
        int result,
        CancellationToken ct = default)
    {
        var entitiesSaved = eventData.Context?.ChangeTracker.Entries<Entity>()
            .Select(po => po.Entity)
            .ToArray();

        if (entitiesSaved == null)
            return result;
        
        App.Log.Information(
            "{EntitiesCount} entities were saved on dbContext {DbHashCode}", 
            entitiesSaved.Length,
            eventData.Context!.GetHashCode());
        
        foreach (var entity in entitiesSaved)
        {
            App.Log.Information("Saved entity {Entity}", entity);
        }
            
        return await Task.FromResult(result);
    }
}