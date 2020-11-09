using Microsoft.AspNetCore.Mvc;

namespace Miru.Turbolinks
{
    public class TurbolinksRedirectResult : ContentResult
    {
        private readonly string _location;

        public TurbolinksRedirectResult(string location)
        {
            _location = location;
            
            ContentType = "text/javascript";
            Content = $@"
Turbolinks.clearCache()
Turbolinks.visit(""{location}"", ""advance"")";
            StatusCode = 200;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.Headers["X-Xhr-Redirect"] = _location;
            
            base.ExecuteResult(context);
        }
    }
}