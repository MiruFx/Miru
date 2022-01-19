using System.Collections.Generic;
using System.Linq;

namespace Miru.Domain;

public interface INameable
{
    string Name { get; }
}

public static class NameableExtensions
{
    public static T ByName<T>(this IEnumerable<T> list, string name) where T : INameable => 
        list.SingleOrDefault(x => x.Name == name);
}