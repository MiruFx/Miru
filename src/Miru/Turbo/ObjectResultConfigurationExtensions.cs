using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mvc;
using Miru.Urls;
using Vereyon.Web;

namespace Miru.Turbo
{
    public static class ObjectResultConfigurationExtensions
    {
        public static void MiruTurbo(this ObjectResultConfiguration _)
        {
            _.When(m => m.Request.CanAccept(TurboStreamResult.MimeType) && 
                        m.Request.IsGet()).Respond(m =>
            {
                return new ViewResult
                {
                    ViewName = m.GetCurrentActionName(),
                    ViewData = m.GetViewData()
                };
            });

            _.When(m => m.Request.CanAccept(TurboStreamResult.MimeType) && 
                        m.Model is IRedirect && 
                        m.Request.IsPost()).Respond(m =>
            {
                var redirect = (IRedirect) m.Model;
                
                var urlLookup = m.ActionContext.HttpContext.RequestServices.GetRequiredService<UrlLookup>();

                var redirectToUrl = redirect.RedirectTo is string url ? url : urlLookup.For(redirect.RedirectTo);
                
                return new RedirectResult(redirectToUrl);
            });
            
            _.When(m => m.Request.CanAccept(TurboStreamResult.MimeType) && 
                        m.Model is FeatureResult && 
                        m.Request.IsPost()).Respond(m =>
            {
                var feature = (FeatureResult) m.Model;

                FlashResultMessages(feature, m);

                var urlLookup = m.ActionContext.HttpContext.RequestServices.GetRequiredService<UrlLookup>();

                var redirectToUrl = urlLookup.For(feature.Model);
                
                return new RedirectResult(redirectToUrl);
            });
            
            _.When(m => m.Request.CanAccept(TurboStreamResult.MimeType) && 
                        m.Request.IsPost()).Respond(m => 
                new PartialViewResult
                {
                    ViewName = $"_{m.GetCurrentActionName()}.turbo",
                    ViewData = m.GetViewData(),
                    ContentType = TurboStreamResult.MimeType
                });
        }

        private static void FlashResultMessages(FeatureResult feature, ObjectResultContext m)
        {
            if (feature.Messages.Any())
            {
                var flashMessage = m.GetService<IFlashMessage>();

                foreach (var message in feature.Messages)
                {
                    switch (message.Key)
                    {
                        case FlashMessageType.Confirmation:
                            flashMessage.Confirmation(message.Value);
                            break;
                        case FlashMessageType.Danger:
                            flashMessage.Danger(message.Value);
                            break;
                        case FlashMessageType.Warning:
                            flashMessage.Warning(message.Value);
                            break;
                        case FlashMessageType.Info:
                            flashMessage.Info(message.Value);
                            break;
                    }
                }
            }
        }
    }
}