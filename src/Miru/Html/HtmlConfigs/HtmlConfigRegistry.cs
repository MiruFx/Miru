using Microsoft.Extensions.DependencyInjection;
using Miru.Html.HtmlConfigs.Core;
using Miru.Html.Tags;

namespace Miru.Html.HtmlConfigs;

public static class HtmlConfigRegistry
{
    public static IServiceCollection AddHtmlConventions(
        this IServiceCollection services,
        HtmlConventions htmlConventions)
    {
        services
            .AddSingleton<ElementNaming>()
            .AddSingleton<TagModifier>()
            .AddSingleton<TagServices>()
            .AddSingleton<TagHelperModifier>()
            .AddTransient<IAntiforgeryAccessor, AntiForgeryAccessor>();
        
        return services.AddSingleton(htmlConventions);
    }
    
    public static IServiceCollection AddHtmlConventions<TConfig>(
        this IServiceCollection services) where TConfig : HtmlConventions, new() =>
            services.AddHtmlConventions(new TConfig());
}