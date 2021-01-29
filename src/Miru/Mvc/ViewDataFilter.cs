using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Miru.Mvc
{
    public class ViewDataFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = (Controller) context.Controller;

            context.HttpContext.Items["Miru_ViewData"] = controller.ViewData;
            
            base.OnResultExecuting(context);
        }
    }
}