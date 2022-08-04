namespace Miru;

public static class BooleanExtensions
{
    private const string TruePascal = "True";
    private const string TrueLower = "true";
    private const string FalsePascal = "False";
    private const string FalseLower = "false";

    public static bool IsEqual(this bool value, string text)
    {
        if (value)
            return text.Equals(TruePascal) || text.Equals(TrueLower);

        return text.Equals(FalsePascal) || text.Equals(FalseLower); 
    }
    public static TReturn IfTrueThen<TReturn>(this bool value, TReturn @return)
    {
        return value ? @return : default;
    }
        
    public static TReturn IfFalseThen<TReturn>(this bool value, TReturn @return)
    {
        return value == false ? @return : default;
    }
}