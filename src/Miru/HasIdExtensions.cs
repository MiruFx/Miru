using System.Collections.Generic;
using System.Linq;
using Miru.Domain;

namespace Miru;

public static class HasIdExtensions
{
    public static bool IsNew(this IHasId hasId) => hasId.Id == 0;
        
    public static bool IsNotNew(this IHasId hasId) => hasId.Id != 0;
    
    public static TList ById<TList>(this IEnumerable<TList> list, long id) where TList : IHasId
    {
        var item = list.FirstOrDefault(e => e.Id == id);

        if (item is null)
            throw new NotFoundException($"Could not find item with id #{id}");

        return item;
    }
}