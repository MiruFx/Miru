using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MiruNext.Framework.FeaturesFolder;

public class FeatureViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        var featureName = GetFeatureName(context);

        foreach (var location in viewLocations)
        {
            yield return location.Replace("{Feature}", featureName);
        }
    }

    private string GetFeatureName(ViewLocationExpanderContext context)
    {
        var endpoint = context.ActionContext.ActionDescriptor.Properties[0];

        var featureInfo = new FeatureInfo(endpoint);
        
        return featureInfo.FeatureGroup;
    }
}