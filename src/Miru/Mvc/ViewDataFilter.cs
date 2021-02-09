using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Miru.Pipeline;

namespace Miru.Mvc
{
    public class ViewDataFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                var miruViewData = context.HttpContext.RequestServices.GetRequiredService<MiruViewData>();

                foreach (var item in miruViewData)
                {
                    controller.ViewData.AddOrUpdate(item.Key, item.Value);
                }
                
                context.HttpContext.Items["Miru_ViewData"] = controller.ViewData;
                context.HttpContext.Items["Miru_TempData"] = controller.TempData;
            }
            
            base.OnResultExecuting(context);
        }
    }
}