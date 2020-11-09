using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc
{
    public abstract class ExceptionResultConfig
    {
        public abstract ActionResult Handle(ExceptionResultContext context);
        
        protected ActionResult NotHandled()
        {
            return new NotHandledResult();
        }

        protected ActionResult StatusCode(HttpStatusCode statusCode)
        {
            return new StatusCodeResult((int) statusCode); 
        }
    }
}