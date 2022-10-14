using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.UserStamp;

namespace Miru;

public static class UserStampRegistry
{
    public static IServiceCollection AddUserStamp(this IServiceCollection services)
    {
        return services.AddTransient<IInterceptor, UserStampInterceptor>();
    }
}