using System.Linq;
using SelfImprov.Domain;
using Z.EntityFramework.Plus;

namespace SelfImprov.Database
{
    public static class QueryExtensions
    {
        public static IQueryable<Area> IncludeGoals(this IQueryable<Area> queryable)
        {
            return queryable.IncludeFilter(m => m.Goals.Where(g => g.IsInactive == false));
        }
    }
}