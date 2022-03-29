using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Miru.Userfy;

namespace Miru.Queuing;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseQueueAdminDashboard<TUser>(this IApplicationBuilder app) 
        where TUser : UserfyUser
    {
        return app.UseHangfireDashboard("/_queue", new DashboardOptions
        {
            Authorization = new [] { new OnlyAdminFilter<TUser>() },
        });
    }
        
    public static IApplicationBuilder UseQueueDashboard(this IApplicationBuilder app)
    {
        return app.UseHangfireDashboard("/_queue");
    }
}