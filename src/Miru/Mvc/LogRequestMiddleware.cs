using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Miru.Mvc
{
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        
        public LogRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch(); 
            
            watch.Start();
            
            App.Framework.Debug($"New request {context.Request.Method} {context.Request.GetEncodedPathAndQuery()} TraceId: {context.TraceIdentifier}");
            
            await _next(context);
            
            watch.Stop();
            
            App.Framework.Debug($"Request returned {context.Response.StatusCode} in {watch.ElapsedMilliseconds} ms");
        }
    }
}
