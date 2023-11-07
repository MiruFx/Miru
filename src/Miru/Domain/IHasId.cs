using System.Collections.Generic;
using System.Linq;

namespace Miru.Domain;

public interface IHasId
{
    long Id { get; }
}

public static class HasIdExtensions
{
    public static string IdsByComma<T>(this IEnumerable<T> items) where T : IHasId =>
        items.Select(x => x.Id).Join(", ");
    
    public static long[] Ids<T>(this IEnumerable<T> items) where T : IHasId =>
        items.Select(x => x.Id).ToArray();
}