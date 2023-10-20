using System.Reflection;

namespace Miru;

public static class ObjectExtensions
{
    public static long ToLong(this object value)
    {
        if (value == null)
            return default;

        if (value is long number)
            return number;
        
        return Convert.ToInt64(value);;
    }
        
    public static int ToInt(this Enum value)
    {
        return Convert.ToInt32(value);
    }
        
    public static int ToInt(this object value)
    {
        if (value == null)
            return default;

        if (value is int number)
            return number;
        
        return Convert.ToInt32(value);
    }
        
    // public static TAttribute GetAttribute<TAttribute>(this object instance) 
    //     where TAttribute : Attribute
    // {
    //     return instance
    //         .GetType()
    //         .GetCustomAttribute<TAttribute>();
    // }
        
    // TODO: move to another class, not any object extension
    public static object GetPropertyValue<T>(this T @this, string propertyName)
    {
        Type type = @this.GetType();
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

        return property.GetValue(@this, null);
    }
    
    public static bool NotEquals<T>(this T current, T other) => 
        current.Equals(other) == false;
}