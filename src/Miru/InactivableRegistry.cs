using System.Linq;
using Miru.Domain;
using Z.EntityFramework.Plus;

namespace Miru;

public static class InactivableRegistry
{
    public static IServiceCollection AddInactivable(this IServiceCollection services)
    {
        QueryFilterManager.Filter<IInactivable>(nameof(IInactivable), q => q.Where(x => x.IsInactive == false));
            
        return services;
    }
}