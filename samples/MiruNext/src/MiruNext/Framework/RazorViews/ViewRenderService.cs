using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Miru.Core;

namespace MiruNext.Framework.RazorViews;

public class ViewRenderService
{
    private readonly IActionResultExecutor<ViewResult> _executor;

    public ViewRenderService(
        IActionResultExecutor<ViewResult> executor)
    {
        _executor = executor;
    }

    public async Task RenderAsync<TModel>(string partialName, TModel model, BaseEndpoint endpoint)
    {
        var actionContext = new ActionContext(
            endpoint.HttpContext, 
            new RouteData(), 
            new ActionDescriptor());

        actionContext.ActionDescriptor.Properties[0] = endpoint;
        
        var viewDataDictionary = new ViewDataDictionary<TModel>(
            metadataProvider: new EmptyModelMetadataProvider(), 
            modelState: new ModelStateDictionary())
        {
            Model = model
        };
        
        var viewResult = new ViewResult
        {
            ViewName = partialName,
            ViewData = viewDataDictionary
        };
        
        Console2.WhiteLine(actionContext.HttpContext.Response.StatusCode.ToString());

        await _executor.ExecuteAsync(actionContext, viewResult);
    }
}