using Microsoft.AspNetCore.Builder;

namespace Miru.Mvc
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionLoggerMiddleware>();
        }
        
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogRequestMiddleware>();
        }
    }
}