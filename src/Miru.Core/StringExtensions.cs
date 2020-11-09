using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;

namespace Miru.Core
{
    public static class StringExtensions
    {
        public static string IfThen(this string value, string ifValue, string thenValue)
        {
            if (value != null && value.Equals(ifValue))
                return thenValue;

            return value;
        }
        
        private static readonly string[] BreakCamelCasePatterns =
        {
            "([a-z])([A-Z])",
            "([0-9])([a-zA-Z])",
            "([a-zA-Z])([0-9])"
        };

        public static bool StartsWith(this string text, params string[] compareWith)
        {
            return compareWith.Any(text.StartsWith);
        }

        public static bool EndsWith(this string text, params string[] compareWith)
        {
            return compareWith.Any(text.EndsWith);
        }
        
        public static bool CaseCmp(this string content, string compareWith)
        {
            return content.Equals(compareWith, StringComparison.OrdinalIgnoreCase);
        }
        
        public static bool CaseCmp(this string content, params string[] compareWith)
        {
            foreach (var compare in compareWith)
            {
                if (content.Equals(compare, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }

        public static bool ContainsNoCase(this string content, string compareWith)
        {
            return content.Contains(compareWith, StringComparison.OrdinalIgnoreCase);
        }
        
        public static string RemoveAtTheEnd(this string text, int count)
        {
            return text.Substring(0, text.Length - count);
        }
        
        public static string BreakCamelCase(this string text)
        {
            var output = BreakCamelCasePatterns.Aggregate(
                text, 
                (current, pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));
            
            return output.Replace('_', ' ');
        }
        
        public static string ToKebabCase(this string source)
        {
            return source.Kebaberize().Replace('.', '-');
        }
    }
}
