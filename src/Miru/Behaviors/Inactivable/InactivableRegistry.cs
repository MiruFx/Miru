using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Miru.Domain;
using Z.EntityFramework.Plus;

namespace Miru.Behaviors.Inactivable
{
    public static class InactivableRegistry
    {
        public static IServiceCollection AddInactivable(this IServiceCollection services)
        {
            QueryFilterManager.Filter<IInactivable>(q => q.Where(x => x.IsInactive == false));
            
            return services;
        }
    }
}
