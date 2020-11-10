using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miru
{
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
        
        public static string FeatureName(this Type type)
        {
            if (type.ReflectedType != null)
                return type.ReflectedType.Name;

            return type.Name;
        }
        
        public static bool IsGenericOf(this Type type, Type otherType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == otherType;
        }
        
        public static bool Implements(this Type type, Type otherType)
        {
            return otherType.IsAssignableFrom(type);
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
        
        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);

            return null;
        }
        
        public static bool IsRequestQuery(this Type type)
        {
            return type.Name.Equals(Query); // type.Implements<IBaseRequest>()
        }
        
        public static bool IsRequestCommand(this Type type)
        {
            return type.Name.Equals(Command); // type.Implements<IBaseRequest>()
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
}