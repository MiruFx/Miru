using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc;

public static class ObjectResultConfigurationExtensions
{
    public static void MiruJson(this ObjectResultConfiguration _)
    {
        _.When(m => m.Request.CanAccept(MediaTypeNames.Application.Xml))
            .Respond(m => new JsonResult(m.Model));
    }
        
    public static void MiruDefault(this ObjectResultConfiguration _)
    {
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