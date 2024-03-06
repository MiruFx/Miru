using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miru.Core;

public static class TypeExtensions
{
    
    public static bool IsGenericOf(this Type type, Type otherType)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == otherType;
    }
        
    public static object GetDefault(this Type type)
    {
        object output = null;

        if (type.IsValueType)
        {
            output = Activator.CreateInstance(type);
        }

        return output;
    }
        
    public static bool Implements(this Type type, Type otherType)
    {
        return otherType.IsAssignableFrom(type);
    }
        
    public static bool IsAnyOf<TType1, TType2>(this Type type)
    {
        return type == typeof(TType1) || type == typeof(TType2);
    }
        
    public static bool Implements<TOtherType>(this Type type)
    {
        return typeof(TOtherType).IsAssignableFrom(type);
    }
        
    public static bool ImplementsGenericOf(this Type type, Type otherType)
    {
        return (type.IsGenericType && type.GetGenericTypeDefinition() == otherType) || 
               (type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == otherType)) ||
               (type.BaseType != null && type.BaseType.IsGenericType && type.BaseType?.GetGenericTypeDefinition() == otherType);
    }
        
    public static bool ImplementsEnumerableOfSomething(this Type type)
    {
        return type
            .GetInterfaces()
            .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }
        
    public static bool ImplementsEnumerableOf<TType>(this Type type)
    {
        var implementsOnInterfaces = type
            .GetInterfaces()
            .Any(t => 
                t.IsGenericType && 
                t.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                t.GetGenericArguments().Any(a => a == typeof(TType) || a.Implements<TType>()));

        var isItself = type.IsGenericType &&
                       type.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                       type.GetGenericArguments().Any(a => a == typeof(TType) || a.Implements<TType>());

        return implementsOnInterfaces || isItself;
    }
        
    public static bool IsGenericListOfEnum(this Type type)
    {
        return (type.IsInterface ? new[] { type } : type.GetInterfaces())
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Any(i => i.GetGenericArguments().First().IsEnum);
    }
        
    public static object GetDefaultValue2(this Type t)
    {
        if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
            return Activator.CreateInstance(t);

        return null;
    }

    public static bool IsAssignableTo(this Type type, Type otherType)
    {
        var typeInfo = type.GetTypeInfo();
        var otherTypeInfo = otherType.GetTypeInfo();

        if (otherTypeInfo.IsGenericTypeDefinition)
        {
            return typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo);
        }

        return otherTypeInfo.IsAssignableFrom(typeInfo);
    }
        
    private static bool IsAssignableToGenericTypeDefinition(this TypeInfo typeInfo, TypeInfo genericTypeInfo)
    {
        var interfaceTypes = typeInfo.ImplementedInterfaces.Select(t => t.GetTypeInfo());

        foreach (var interfaceType in interfaceTypes)
        {
            if (interfaceType.IsGenericType)
            {
                var typeDefinitionTypeInfo = interfaceType
                    .GetGenericTypeDefinition()
                    .GetTypeInfo();

                if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
                {
                    return true;
                }
            }
        }

        if (typeInfo.IsGenericType)
        {
            var typeDefinitionTypeInfo = typeInfo
                .GetGenericTypeDefinition()
                .GetTypeInfo();

            if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
            {
                return true;
            }
        }

        var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();

        if (baseTypeInfo is null)
        {
            return false;
        }

        return baseTypeInfo.IsAssignableToGenericTypeDefinition(genericTypeInfo);
    }
}