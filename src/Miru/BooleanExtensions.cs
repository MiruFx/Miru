namespace Miru;

public static class BooleanExtensions
{
    private const string True = "true";
    private const string False = "false";
    
    public static string If(this string contentToShow, bool condition) =>
        condition ? contentToShow : string.Empty;
    
    public static string IfNot(this string contentToShow, bool condition) =>
        condition == false ? contentToShow : string.Empty;
    
    public static TReturn IfTrueThen<TReturn>(this bool value, TReturn @return)
    {
        return value ? @return : default;
    }
        
    public static TReturn IfFalseThen<TReturn>(this bool value, TReturn @return)
    {
        return value == false ? @return : default;
    }
}