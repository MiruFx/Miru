using System;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using HtmlTags.Conventions.Elements.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Html;

namespace Miru;

public static class HtmlRegistry
{
    public static IServiceCollection AddMiruHtml(
        this IServiceCollection services)
    {
        return services.AddMiruHtml(x => x.AddMiruDefault());
    }
    
    public static IServiceCollection AddMiruHtml(
        this IServiceCollection services,
        Action<HtmlConfiguration> action)
    {
        var htmlConfig = new HtmlConfiguration();

        action(htmlConfig);

        return services.AddMiruHtml(htmlConfig);
    }
    
    public static IServiceCollection AddMiruHtml<THtmlConfig>(
        this IServiceCollection services) 
        where THtmlConfig : HtmlConfiguration, new()
    {
        return services.AddMiruHtml(new THtmlConfig());
    }
    
    public static IServiceCollection AddMiruHtml(
        this IServiceCollection services, 
        HtmlConfiguration htmlConfig)
    {
        services.TryAddSingleton<HtmlGenerator>();

        services.TryAddSingleton<ElementNaming>();
        services.TryAddSingleton<IElementNamingConvention, DotNotationElementNamingConvention>();
            
        services.TryAddTransient<IAntiforgeryAccessor, AntiForgeryAccessor>();
            
        var library = new HtmlConventionLibrary();

        // TODO: from here down is html configuration. Should not be here I guess 
        var defaultRegistry = new HtmlConventionRegistry()
            .DefaultModifiers();

        defaultRegistry.Apply(library);

        htmlConfig.Apply(library);

        var defaultBuilders = new HtmlConventionRegistry();
            
        defaultBuilders.Editors.Always.BuildBy<TextboxBuilder>();
        defaultBuilders.Labels.Always.BuildBy<DefaultLabelBuilder>();
        defaultBuilders.DefaultNamingConvention();
        defaultBuilders.Displays.Always.BuildBy<SpanDisplayBuilder>();
        defaultBuilders.Displays.NamingConvention(new DotNotationElementNamingConvention());
            
        defaultBuilders.Apply(library);
            
        return services.AddHtmlTags(library);
    }
}