using System;
using System.Linq;
using System.Reflection;

namespace Miru.Core;

public static class TypeExtensions
{
    // public static string GetName(this Type type)
    // {
    //     if (!type.GetTypeInfo().IsGenericType)
    //         return type.Name;
    //     
    //     var str = string.Join(", ", type.GetGenericArguments().Select((Func<Type, string>) (x => x.GetName())).ToArray());
    //     
    //     return $"{type.Name}<{str}>";
    // }
}