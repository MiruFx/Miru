using System.Collections.Generic;
using System.Linq;

namespace Miru.Domain;

public interface ICheckable
{
    bool IsChecked { get; set; }
}

public static class CheckableExtensions
{
    public static IEnumerable<TModel> AllChecked<TModel>(this IEnumerable<TModel> items) 
        where TModel : ICheckable
    {
        return items.Where(x => x.IsChecked);
    }
        
    public static IEnumerable<TModel> AllUnchecked<TModel>(this IEnumerable<TModel> items) 
        where TModel : ICheckable
    {
        return items.Where(x => x.IsChecked == false);
    }
}