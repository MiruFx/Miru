namespace Miru;

public static class NullableExtensions
{
    public static bool HasNoValue<T>(this T? nullable) where T : struct =>
        nullable.HasValue == false;
}