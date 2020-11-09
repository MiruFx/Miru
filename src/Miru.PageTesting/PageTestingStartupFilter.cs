using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Miru.PageTesting
{
    public class PageTestingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // app.UseAuthentication();
                // app.UseMiddleware<OnNextRequestMiddleware>();

                next(app);
            };
        }
    }
}