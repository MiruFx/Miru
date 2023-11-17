using System.Collections.Generic;
using System.Linq;
using Miru.Domain;

namespace Miru;

public static class EnumerableExtensions
{
    public static T SingleOrFail<T>(this IEnumerable<T> enumerable, string exceptionMessage)
    {
        return enumerable.SingleOrDefault() ?? throw new NotFoundException(exceptionMessage);
    }
}