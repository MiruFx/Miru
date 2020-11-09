using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Miru.Userfy
{
    public static class OnNextRequest
    {
        public static Action<HttpContext> Execute;
    }
    
    public class OnNextRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public OnNextRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (OnNextRequest.Execute != null)
            {
                OnNextRequest.Execute(context);
                OnNextRequest.Execute = null;
            }

            await _next(context);
        }
    }
}