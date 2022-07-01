using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Miru.Queuing;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseQueueDashboard(
        this IApplicationBuilder app,
        string queuePath,
        Action<DashboardOptions> config = null)
    {
        return app.UseQueueDashboard(queuePath, "en-IE", config: config);
    }
    
    public static IApplicationBuilder UseQueueDashboard(
        this IApplicationBuilder app,
        string queuePath = "/_queue",
        string culture = "en-IE",
        Action<DashboardOptions> config = null)
    {
        var options = new DashboardOptions
        {
            AsyncAuthorization = new[] { new HangfireAuthorizationFilter() }
        };
        
        config?.Invoke(options);
        
        return app.MapWhen(context => context.Request.Path.StartsWithSegments(queuePath), appBuilder =>
        {
            appBuilder.UseRequestLocalization(culture);
            
            appBuilder.UseHangfireDashboard(queuePath, options);
        });
    }
}