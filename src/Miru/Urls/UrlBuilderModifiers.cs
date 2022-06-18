using System.Collections.Generic;
using System.Linq;
using Baseline.Reflection;

namespace Miru.Urls;

public class UrlBuilderModifiers
{
    public Dictionary<string, object> With { get; }
    public List<string> Without { get; }
    public List<KeyValuePair<string, object>> WithoutValues { get; }
    public bool HasModifiers => With.Any() || Without.Any() || WithoutValues.Any();

    public UrlBuilderModifiers()
    {
        With = new();
        Without = new();
        WithoutValues = new();
    }
    
    public UrlBuilderModifiers(
        Dictionary<string, object> with, 
        List<string> without, 
        List<KeyValuePair<string, object>> withoutValues)
    {
        With = with;
        Without = without;
        WithoutValues = withoutValues;

        // HasModifiers = With.Any() || Without.Any() || WithoutValues.Any();
    }
}