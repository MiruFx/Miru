using System.Collections.Generic;
using System.Linq;
using Baseline.Reflection;

namespace Miru.Urls;

public class UrlBuilderModifiers
{
    public Dictionary<string, object> With { get; }
    public List<string> Without { get; }
    public List<KeyValuePair<string, object>> WithoutValues { get; }
    public Dictionary<string,object> WithOnly { get; }
    
    public bool HasModifiers => With.Any() || Without.Any() || WithoutValues.Any() || WithOnly.Any();

    public UrlBuilderModifiers()
    {
        With = new();
        Without = new();
        WithoutValues = new();
        WithOnly = new();
    }
    
    public UrlBuilderModifiers(
        Dictionary<string, object> with,
        List<string> without,
        List<KeyValuePair<string, object>> withoutValues, 
        Dictionary<string, object> withOnlyProperties)
    {
        With = with;
        Without = without;
        WithoutValues = withoutValues;
        WithOnly = withOnlyProperties;
    }
}