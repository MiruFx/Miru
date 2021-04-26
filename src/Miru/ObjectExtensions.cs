using System;
using System.Collections.Generic;
using System.Linq;
using Baseline.Reflection;
using Miru.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Miru
{
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
    }
}
