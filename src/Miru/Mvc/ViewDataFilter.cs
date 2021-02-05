using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Miru.Mvc
{
    public class ViewDataFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                context.HttpContext.Items["Miru_ViewData"] = controller.ViewData;
                context.HttpContext.Items["Miru_TempData"] = controller.TempData;    
            }
            
            base.OnResultExecuting(context);
        }
    }
}