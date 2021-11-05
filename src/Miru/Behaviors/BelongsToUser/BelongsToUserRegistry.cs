using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Behaviors.BelongsToUser
{
    public static class BelongsToUserServiceCollectionExtensions
    {
        public static IServiceCollection AddBelongsToUser(this IServiceCollection services)
        {
            return services
                .AddScoped<IQueryFilter, BelongsToUserFilter>()
                .AddScoped<IInterceptor, BelongsToUserInterceptor>();
        }
    }
}
