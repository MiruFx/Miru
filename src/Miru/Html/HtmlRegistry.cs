using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Conventions.Elements.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html;

public static class HtmlRegistry
{
    public static IServiceCollection AddMiruHtml(
        this IServiceCollection services, 
        params HtmlConventionRegistry[] registries)
    {
        services.AddSingleton<HtmlGenerator>();

        services.AddSingleton<ElementNaming>();
        services.AddSingleton<IElementNamingConvention, DotNotationElementNamingConvention>();
            
        services.AddSingleton<HtmlGenerator, HtmlGenerator>();
            
        services.AddTransient<IAntiforgeryAccessor, AntiForgeryAccessor>();
            
        var library = new HtmlConventionLibrary();

        var defaultRegistry = new HtmlConventionRegistry()
            .DefaultModifiers();

        defaultRegistry.Apply(library);

        if (registries.Length > 0)
            foreach (var registry in registries)
                registry.Apply(library);
        else
            new HtmlConfiguration().Apply(library);

        var defaultBuilders = new HtmlConventionRegistry();
            
        // defaultBuilders.Editors.BuilderPolicy<CheckboxBuilder>();
        defaultBuilders.Editors.Always.BuildBy<TextboxBuilder>();
        defaultBuilders.Labels.Always.BuildBy<DefaultLabelBuilder>();
        defaultBuilders.DefaultNamingConvention();
        defaultBuilders.Displays.Always.BuildBy<SpanDisplayBuilder>();
        defaultBuilders.Displays.NamingConvention(new DotNotationElementNamingConvention());
            
        defaultBuilders.Apply(library);
            
        return services.AddHtmlTags(library);
    }
}