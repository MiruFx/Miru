using System.Threading.Tasks;
using Baseline;
using Microsoft.AspNetCore.Http;

namespace Miru.Foundation.Logging
{
    public class LogHeadersMiddleware 
    {
        private readonly RequestDelegate _next;

        public LogHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers
                .Each(x => App.Log.Information($"\t{x.Key}: {x.Value}"));
            
            await _next.Invoke(context);
            
            context.Response.Headers
                .Each(x => App.Log.Information($"\t{x.Key}: {x.Value}"));
        }
    }
}