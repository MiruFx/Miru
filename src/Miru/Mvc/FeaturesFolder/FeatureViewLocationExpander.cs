using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Miru.Mvc.FeaturesFolder;

public class FeatureViewLocationExpander : IViewLocationExpander
{
    private readonly string _placeholder;

    public FeatureViewLocationExpander(FeatureFolderOptions options)
    {
        _placeholder = options.FeatureNamePlaceholder;
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        // see: https://stackoverflow.com/questions/36802661/what-is-iviewlocationexpander-populatevalues-for-in-asp-net-core-mvc
        context.Values["action_displayname"] = context.ActionContext.ActionDescriptor.DisplayName;
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (viewLocations == null)
            throw new ArgumentNullException(nameof(viewLocations));

        var controllerDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
        
        var featureName = GetFeatureName(controllerDescriptor, context);

        foreach (var location in viewLocations)
        {
            yield return location.Replace(_placeholder, featureName);
        }
    }

    private string GetFeatureName(
        ControllerActionDescriptor controllerDescriptor,
        ViewLocationExpanderContext context)
    {
        if (controllerDescriptor is null)
        {
            var endpoint = context.ActionContext.ActionDescriptor.Properties[0];

            return "Orders";
        }
        
        return controllerDescriptor?.Properties["feature"] as string;
    }
}