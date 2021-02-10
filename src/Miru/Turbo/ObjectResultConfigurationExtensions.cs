using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mvc;
using Miru.Urls;

namespace Miru.Turbo
{
    public static class ObjectResultConfigurationExtensions
    {
        public static void MiruTurbo(this ObjectResultConfiguration _)
        {
            _.When(m => m.Request.CanAccept(TurboStream.MimeType) && 
                        m.Request.IsGet()).Respond(m =>
            {
                return new PartialViewResult
                {
                    ViewName = m.GetCurrentActionName(),
                    ViewData = m.GetViewData()
                };
            });

            _.When(m => m.Request.CanAccept(TurboStream.MimeType) && 
                        m.Model is IRedirect && 
                        m.Request.IsPost()).Respond(m =>
            {
                var redirect = (IRedirect) m.Model;
                
                var urlLookup = m.ActionContext.HttpContext.RequestServices.GetRequiredService<UrlLookup>();

                var redirectToUrl = redirect.RedirectTo is string url ? url : urlLookup.For(redirect.RedirectTo);
                
                return new RedirectResult(redirectToUrl);
            });
            
            _.When(m => m.Request.CanAccept(TurboStream.MimeType) && 
                        m.Request.IsPost()).Respond(m => 
                new PartialViewResult
                {
                    ViewName = $"_{m.GetCurrentActionName()}.turbo",
                    ViewData = m.GetViewData(),
                    ContentType = TurboStream.MimeType
                });
        }
    }
}