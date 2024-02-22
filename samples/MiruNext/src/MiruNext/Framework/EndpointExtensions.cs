using Microsoft.AspNetCore.Http.HttpResults;
using MiruNext.Framework.Routing;

namespace MiruNext.Framework;

public static class EndpointExtensions
{
    public static readonly object NoModelDictionary = new { }.ToDictionary();
    
    public static async Task View<TComponent>(
        this BaseEndpoint baseEndpoint, 
        object model = null)
    {
        var modelDictionary = model?.ToDictionary() ?? NoModelDictionary;
        
        var modelType = typeof(TComponent);
        
        var result = new RazorComponentResult(
            typeof(PageComponent), 
            new
            {
                ComponentType = modelType, 
                ComponentParameters = modelDictionary, 
                // Errors = errors
            });
        
        await baseEndpoint.HttpContext.Response.SendResultAsync(result);
    }
}