using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.TimeStamp;

namespace Miru;

public static class TimeStampedRegistry
{
    public static IServiceCollection AddTimeStamp(this IServiceCollection services)
    {
        return services.AddTransient<IInterceptor, TimeStampInterceptor>();
    }
}