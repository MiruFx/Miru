using Miru.Domain;

namespace Miru;

public static class HasIdExtensions
{
    public static bool IsNew(this IHasId hasId) => hasId.Id == 0;
        
    public static bool IsNotNew(this IHasId hasId) => hasId.Id != 0;
}