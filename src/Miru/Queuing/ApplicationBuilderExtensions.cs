using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Miru.Userfy;

namespace Miru.Queuing;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseQueueDashboard(
        this IApplicationBuilder app,
        string queuePath = "/_queue",
        string culture = "en-IE")
    {
        return app.MapWhen(context => context.Request.Path.StartsWithSegments(queuePath), appBuilder =>
        {
            appBuilder.UseRequestLocalization("en-IE");
            appBuilder.UseHangfireDashboard(culture, new DashboardOptions
            {
                AsyncAuthorization = new[] { new HangfireAuthorizationFilter() }
            });
        });
    }
}