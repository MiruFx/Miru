using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Security;
using Miru.Userfy;
using Vereyon.Web;

namespace Miru.Mvc;

public static class ExceptionResultConfigurationExtensions
{
    public static void MiruDefault(this ExceptionResultConfiguration _)
    {
        _.When(m => m.Exception is NotFoundException && m.Request.IsGet()).Respond(m => 
            new StatusCodeResult((int) HttpStatusCode.NotFound));
            
        // FIXME: maybe should be set in the application
        _.When(m => m.Exception is UnauthorizedException && m.UserSession.IsAnonymous).Respond(m =>
        {
            var userfyOptions = m.GetService<UserfyOptions>();
                
            m.Flash().Warning(userfyOptions.RequiredLoginMessage);
                    
            return new RedirectResult(
                $"{m.CookieAuthenticationOptions().LoginPath}?ReturnUrl={m.Request.GetEncodedPathAndQuery()}");
        });
            
        _.When(m => m.Exception is UnauthorizedException).Respond(m => 
            new StatusCodeResult((int) HttpStatusCode.Forbidden));
    }
}