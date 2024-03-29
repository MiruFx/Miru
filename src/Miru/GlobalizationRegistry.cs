using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Miru;

public static class GlobalizationRegistry
{
    public static IServiceCollection AddGlobalization(
        this IServiceCollection services, 
        string defaultCulture,
        params string[] supportedCultures)
    {
        // https://www.csharp-examples.net/culture-names/
        var allCultures = new[] { defaultCulture }.Union(supportedCultures).ToArray();
            
        return services
            .AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            })

            .AddRequestLocalization(x =>
            {
                x.AddSupportedCultures(allCultures);
                x.AddSupportedUICultures(allCultures);

                x.DefaultRequestCulture = new RequestCulture(defaultCulture);
                    
                var cookieProvider = x.RequestCultureProviders
                    .OfType<CookieRequestCultureProvider>()
                    .First();

                var urlProvider = x.RequestCultureProviders
                    .OfType<QueryStringRequestCultureProvider>()
                    .First();

                x.RequestCultureProviders.Clear();
                x.RequestCultureProviders.Add(cookieProvider);
                x.RequestCultureProviders.Add(urlProvider);
            })

            .AddMvc()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization()
            .Services;
    }
}