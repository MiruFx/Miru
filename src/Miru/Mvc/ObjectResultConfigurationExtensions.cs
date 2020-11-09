using HtmlTags;
using Microsoft.AspNetCore.Mvc;
using Miru.Behaviors;
using Miru.Turbolinks;

namespace Miru.Mvc
{
    public static class ObjectResultConfigurationExtensions
    {
        public static void MiruDefault(this ObjectResultConfiguration _)
        {
            // Will execute the first match
            _.When(m => m.Request.CanAccept("text/html") && m.Request.IsAjax() && m.Model is HtmlTag).Respond(m =>
            {
                var html = (HtmlTag) m.Model;
            
                return new HtmlTagResult(html);
            });
            
            _.When(m => m.Request.CanAccept("text/html") && m.Request.IsAjax()).Respond(m => 
                new PartialViewResult
                {
                    ViewName = $"_{m.GetCurrentActionName()}.js",
                    ViewData = m.GetViewData()
                });

            _.When(m => m.Request.CanAccept("text/html")).Respond(m => 
                new ViewResult { ViewData = m.GetViewData() });
        }

        public static void AddTurbolinks(this ObjectResultConfiguration _)
        {
            _.When(m => m.Request.IsAjax() && m.Request.IsGet() && m.Model is HtmlTag == false).Respond(m => new PartialViewResult
            {
                ViewName = m.GetCurrentActionName(),
                ViewData = m.GetViewData()
            });
            
            _.When(m => m.Model is IRedirectResult && m.Request.IsAjax() && m.Request.IsGet() == false).Respond(m =>
            {
                var redirectTo = (IRedirectResult) m.Model;

                return new TurbolinksRedirectResult(redirectTo.RedirectTo);
            });
        }
    }
}