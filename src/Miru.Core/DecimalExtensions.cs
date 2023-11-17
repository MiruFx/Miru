using System;
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
    
    public static decimal Negative(this decimal value) => 
        value > 0 ? value * -1 : value;
    
    public static int Negative(this int value) => 
        value > 0 ? value * -1 : value;
    
    public static decimal Positive(this decimal value) => 
        value > 0 ? value : value * -1;
    
    public static decimal PercentOf(this decimal value, decimal total) => 
        total != 0 ? value / total * 100 : 0;

    public static decimal PercentOf(this int value, decimal total) => 
        Convert.ToDecimal(value).PercentOf(total);
}