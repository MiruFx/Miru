using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Miru.Mvc.FeaturesFolder;
using MiruNext.Framework;
using MiruNext.Framework.RazorViews;
using FeatureFoldersRegistry = MiruNext.Framework.FeatureFoldersRegistry;

namespace MiruNext;

public static class EndpointExtensions
{
    public static async Task SendViewAsync<TModel>(
        this BaseEndpoint baseEndpoint,
        string viewName,
        TModel model)
    {
        var sp = baseEndpoint.HttpContext.RequestServices;
        
        var renderer = sp.GetRequiredService<ViewRenderService>();

        await renderer.RenderAsync(viewName, model, baseEndpoint);
    }
    
    public static async Task SendViewAsync<TModel>(
        this BaseEndpoint baseEndpoint,
        TModel model)
    {
        var sp = baseEndpoint.HttpContext.RequestServices;
        
        var renderer = sp.GetRequiredService<ViewRenderService>();

        await renderer.RenderAsync(baseEndpoint.GetType().Name, model, baseEndpoint);
    }
}

public static class StartupExtensions
{
    public static IServiceCollection AddRazorViews<TProgram>(this IServiceCollection services)
    {
        services
            .AddRazorPages()
            .AddApplicationPart(typeof(TProgram).Assembly)
            .Services
            
            .AddMvcCore()
            .AddFeatureFolders2()
            .Services
                
            .TryAddSingleton<ViewRenderService>();
        
        return services;
    }
}

// public abstract partial class Endpoint<TRequest> : BaseEndpoint where TRequest : notnull, new()
// {
//     public async Task SendViewAsync<TResponse>(TResponse model)
//     {
//         // this._response = model;
//         
//         var sp = this.HttpContext.RequestServices;
//         
//         var renderer = sp.GetRequiredService<ViewRenderService>();
//
//         await renderer.RenderAsync(this.GetType().Name, model, this);
//     }
// }

// public class ViewAttribute : Attribute
// {
//     public string ViewName { get; }
//
//     public ViewAttribute(string viewName)
//     {
//         ViewName = viewName;
//     }
// }