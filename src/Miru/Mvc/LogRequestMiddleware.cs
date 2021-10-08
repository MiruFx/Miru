using System.Diagnostics;
using System.Globalization;
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
            var path = context.Request.GetEncodedPathAndQuery();

            if (path.StartsWith('_'))
            {
                await _next(context);
                return;
            }
            
            var watch = new Stopwatch(); 
            
            watch.Start();
            
            App.Log.Debug($"New request {context.Request.Method} {path} TraceId: {context.TraceIdentifier} Culture: {CultureInfo.CurrentCulture}");
            
            await _next(context);
            
            watch.Stop();
            
            App.Log.Debug($"Request returned {context.Response.StatusCode} in {watch.ElapsedMilliseconds} ms");
        }
    }
}
