using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.TimeStamp;

namespace Miru.Behaviors.UserStamp
{
    public static class UserStampServiceCollectionExtensions
    {
        public static IServiceCollection AddUserStamp(this IServiceCollection services)
        {
            return services.AddTransient<IInterceptor, UserStampInterceptor>();
        }
    }
}
