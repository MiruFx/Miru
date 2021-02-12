using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Behaviors.BelongsToUser
{
    public static class BelongsToUserServiceCollectionExtensions
    {
        public static IServiceCollection AddBelongsToUser(this IServiceCollection services)
        {
            return services.AddTransient<IInterceptor, BelongsToUserInterceptor>();
        }
    }
}
