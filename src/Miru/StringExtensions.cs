namespace Miru
{
    public static class StringExtensions
    {
        public static bool NotEqual(this string current, string value)
        {
            return !current.Equals(value);
        }
        
        public static string IfEmpty(this string value, string valueIfEmpty) =>
            string.IsNullOrEmpty(value) ? valueIfEmpty : value;
    }
}