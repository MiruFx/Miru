using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Mvc
{
    public class ObjectResultContext
    {
        public HttpRequest Request { get; set; }
        public object Model { get; set; }
        public ObjectResult ObjectResult { get; set; }
        public HttpResponse Response { get; set; }
        public ActionContext ActionContext { get; set; }
        public ViewDataDictionary ViewData { get; set; }

        public T GetService<T>() => Request.HttpContext.RequestServices.GetService<T>();
    }

    public static class ObjectResultContextExtensions
    {
        public static ViewDataDictionary GetViewData(this ObjectResultContext ctx)
        {
            var viewData = (ViewDataDictionary) ctx.ActionContext.HttpContext.Items["Miru_ViewData"];

            if (viewData == null)
            {
                var metalDataProvider = ctx.GetService<IModelMetadataProvider>();

                viewData = new ViewDataDictionary(metalDataProvider, ctx.ActionContext.ModelState);
            }

            viewData.Model = ctx.Model;
            
            return viewData;
        }

        public static string GetCurrentActionName(this ObjectResultContext ctx)
        {
            ctx.ActionContext.ActionDescriptor.RouteValues.TryGetValue("action", out var action);
            return action;
        }
    }
}