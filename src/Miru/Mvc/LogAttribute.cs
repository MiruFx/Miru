using Microsoft.AspNetCore.Mvc.Filters;

namespace Miru.Mvc
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //App.Log.Debug("Mvc Result executed");
            base.OnResultExecuted(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //App.Log.Debug("Mvc Action executed");
            base.OnActionExecuted(filterContext);
        }
    }
}
