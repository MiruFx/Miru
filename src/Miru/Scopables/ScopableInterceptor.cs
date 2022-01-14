using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Miru.Scopables;

public class ScopableInterceptor : SaveChangesInterceptor
{
    private readonly IEnumerable<IScopableSaving> _scopableSavings;

    public ScopableInterceptor(IEnumerable<IScopableSaving> scopableSavings)
    {
        _scopableSavings = scopableSavings;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData @event, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        foreach (var scopableSaving in _scopableSavings)
        {
            @event.Context?.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ForEach(x => scopableSaving.OnSaving(x));
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}