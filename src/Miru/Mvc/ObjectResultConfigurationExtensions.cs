using System.Net.Mime;
using HtmlTags;
using Microsoft.AspNetCore.Mvc;
using Miru.Behaviors;

namespace Miru.Mvc
{
    public static class ObjectResultConfigurationExtensions
    {
        public static void MiruJson(this ObjectResultConfiguration _)
        {
            _.When(m => m.Request.CanAccept(MediaTypeNames.Application.Xml))
                .Respond(m => new JsonResult(m.Model));
        }
        
        public static void MiruDefault(this ObjectResultConfiguration _)
        {
            // Will execute the first match
            _.When(m => 
                m.Request.CanAccept("text/html") && 
                m.Request.IsAjax() && 
                m.Model is HtmlTag).Respond(m =>
            {
                var html = (HtmlTag) m.Model;
            
                return new HtmlTagResult(html);
            });
            
            _.When(m => 
                m.Request.CanAccept("text/html") && 
                m.Request.IsAjax()).Respond(m => 
                    new PartialViewResult
                    {
                        ViewName = $"_{m.GetCurrentActionName()}.js",
                        ViewData = m.GetViewData()
                    });

            _.When(m => m.Request.CanAccept("text/html")).Respond(m => 
                new ViewResult
                {
                    ViewData = m.GetViewData()
                });
        }
    }
}