using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miru;

public static class TypeExtensions
{
    private static readonly string Command = nameof(Command);
    private static readonly string Query = nameof(Query);

    public static string ActionName(this Type type)
    {
        if (type.ReflectedType != null)
            return $"{type.ReflectedType.Name}.{type.Name}";

        return type.Name;
    }
                
    public static bool IsRequestQuery(this Type type)
    {
        return type.Name.Equals(Query); // type.Implements<IBaseRequest>()
    }
        
    public static bool IsRequestCommand(this Type type)
    {
        return type.Name.Equals(Command); // type.Implements<IBaseRequest>()
    }

    public static string FeatureName(this Type type)
    {
        if (type.ReflectedType != null)
            return type.ReflectedType.Name;

        return type.Name;
    }
}