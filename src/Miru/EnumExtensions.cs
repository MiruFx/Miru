using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Miru
{
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum @enum) 
        {
            var fieldInfo = @enum.GetType().GetField(@enum.ToString());

            var descriptionAttributes = fieldInfo!
                .GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) 
                return string.Empty;

            return descriptionAttributes.Length > 0 
                ? descriptionAttributes[0].Name 
                : @enum.ToString();
        }
        
        public static IEnumerable<TEnum> All<TEnum>() where TEnum : struct,  IComparable, IFormattable, IConvertible
        {
            var enumType = typeof(TEnum);

            if(!enumType.IsEnum)
                throw new ArgumentException($"The type passed is not an Enum: {enumType.Name}");

            return Enum.GetValues(enumType).Cast<TEnum>();
        }
        
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) 
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }
    }
}