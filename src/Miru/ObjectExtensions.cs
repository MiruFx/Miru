using System;
using System.Reflection;

namespace Miru;

public static class ObjectExtensions
{
    /// <summary>
    /// If current variable is null or default(T), return the other value
    /// </summary>
    public static string Or<T>(this T value, string valueIfNullOrEmpty)
    {
        if (value is string valueString && string.IsNullOrEmpty(valueString))
            return valueIfNullOrEmpty;
                
        if (value == null || value.Equals(default(T)))
            return valueIfNullOrEmpty;

        return value.ToString();
    }
        
    public static long ToLong(this object value)
    {
        long ret = 0;

        if (value == null)
        {
            return ret;
        }

        long.TryParse(value.ToString(), out ret);
        return ret;
    }
        
    public static int ToInt(this Enum value)
    {
        return Convert.ToInt32(value);
    }
        
    public static int ToInt(this object value)
    {
        int ret = 0;

        if (value == null)
            return ret;

        int.TryParse(value.ToString(), out ret);
        return ret;
    }
        
    public static TAttribute GetAttribute<TAttribute>(this object instance) 
        where TAttribute : Attribute
    {
        return instance
            .GetType()
            .GetCustomAttribute<TAttribute>();
    }
        
    public static void With<TInstance>(this TInstance instance, Action<TInstance> action)
    {
        if (instance != null)
            action(instance);
    }
    
    public static object GetPropertyValue<T>(this T @this, string propertyName)
    {
        Type type = @this.GetType();
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        return property.GetValue(@this, null);
    }
}