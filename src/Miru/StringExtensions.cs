namespace Miru
{
    public static class StringExtensions
    {
        public static bool NotEqual(this string current, string value)
        {
            return !current.Equals(value);
        }
    }
}