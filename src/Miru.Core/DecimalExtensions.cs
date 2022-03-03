using System.Globalization;

namespace Miru.Core;

public static class DecimalExtensions
{
    /// <summary>
    /// it converts to string using no culture. Sanus uses German culture, so here it will be converted to
    /// default .net culture (US) that can be used to other components, like javascript 
    /// </summary>
    public static string ToStringInvariant(this decimal value) => 
        value.ToString(CultureInfo.InvariantCulture);
}