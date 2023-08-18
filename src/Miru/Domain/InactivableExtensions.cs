using System.Collections.Generic;
using System.Linq;

namespace Miru.Domain;

public static class InactivableExtensions
{
    public static bool IsActive(this IInactivable inactivable) => 
        inactivable.IsInactive == false;

    public static void ActivateOrInactivate(this IInactivable inactivable) => 
        inactivable.IsInactive = !inactivable.IsInactive;

    public static void Inactivate(this IInactivable inactivable) =>
        inactivable.IsInactive = true;
    
    public static IQueryable<T> AllActive<T>(this IQueryable<T> query) where T : IInactivable =>
        query.Where(x => x.IsInactive == false);
    
    public static IEnumerable<T> AllActive<T>(this IEnumerable<T> query) where T : IInactivable =>
        query.Where(x => x.IsInactive == false);
}