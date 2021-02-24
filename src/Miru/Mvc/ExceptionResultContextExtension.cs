using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Miru.Userfy;
using Vereyon.Web;

namespace Miru.Mvc
{
    public static class ExceptionResultContextExtension
    {
        public static IFlashMessage Flash(this ExceptionResultContext ctx) => 
            ctx.GetService<IFlashMessage>();        
        
        public static UserfyOptions UserfyOptions(this ExceptionResultContext ctx) => 
            ctx.GetService<UserfyOptions>();

        public static CookieAuthenticationOptions CookieAuthenticationOptions(this ExceptionResultContext ctx) => 
            ctx.GetService<IOptionsSnapshot<CookieAuthenticationOptions>>().Get(IdentityConstants.ApplicationScheme);
    }
}