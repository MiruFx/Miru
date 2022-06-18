using System;
using System.Collections.Concurrent;

namespace Miru.Urls;

public static class TypeExtensions
{
    // TODO: move somewhere else?
    //a thread-safe way to hold default instances created at run-time
    private static readonly ConcurrentDictionary<Type, object> TypeDefaults = new();

    public static object GetDefaultValue(this Type type)
    {
        return type.IsValueType
            ? TypeDefaults.GetOrAdd(type, Activator.CreateInstance)
            : null;
    }
}