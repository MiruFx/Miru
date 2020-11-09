using System;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc
{
    public static class ControllerExtensions
    {
        public static ContentResult RedirectJson(this Controller controller, string url)
        {
            return controller.JsonNet(new { redirect = url });
        }

        public static ActionResult RedirectToActionJson<TController>(this TController controller, string action)
            where TController : Controller
        {
            return controller.JsonNet(new
            {
                redirect = controller.Url.Action(action)
            });
        }

        public static ContentResult JsonNet(this Controller controller, object model)
        {
            var serialized = JsonSerializer.Serialize(model);

            return new ContentResult
            {
                Content = serialized,
                ContentType = "application/json"
            };
        }

        public static RedirectToRouteResult RedirectToAction<TController>(
            this TController controller,
            Expression<Action<TController>> action) where TController : Controller
        {
            return null;
            // TODO: Fix
            //return Microsoft.Web.Mvc.ControllerExtensions.RedirectToAction(controller, action);
        }
    }
}
