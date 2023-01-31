using MiruNext.Framework.FeaturesFolder;

namespace MiruNext.Framework;

public static class FeatureFoldersRegistry
{
    public static IMvcCoreBuilder AddFeatureFolders2(this IMvcCoreBuilder services)
    {
        var expander = new FeatureViewLocationExpander();

        services.AddRazorViewEngine(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Features/{Feature}/{0}.cshtml");
                o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                
                o.ViewLocationExpanders.Add(expander);
            });

        return services;
    }
}