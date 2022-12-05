using System;
using System.Linq;
using Baseline.Reflection;

namespace Miru.Html.HtmlConfigs;

public static class AccessorExtensions
{
    public static readonly Func<string, bool> IsCollectionIndexer = 
        x => x.StartsWith('[') && x.EndsWith(']');
    
    public static string GetName(this Accessor accessor) =>
        accessor
            .PropertyNames
            .Aggregate((x, y) => IsCollectionIndexer(y) ? x + y : x + '.' + y);
}