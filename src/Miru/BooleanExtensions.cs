namespace Miru
{
    public static class BooleanExtensions
    {
        public static TReturn IfTrueThen<TReturn>(this bool value, TReturn @return)
        {
            return value ? @return : default;
        }
        
        public static TReturn IfFalseThen<TReturn>(this bool value, TReturn @return)
        {
            return value == false ? @return : default;
        }
    }
}