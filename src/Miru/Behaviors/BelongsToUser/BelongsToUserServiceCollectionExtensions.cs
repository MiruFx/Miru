using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Userfy;

namespace Miru.Behaviors.BelongsToUser
{
    public static class BelongsToUserServiceCollectionExtensions
    {
        public static IServiceCollection AddBelongsToUser<TUser>(this IServiceCollection services)
            where TUser : UserfyUser
        {
            return services.AddTransient<IInterceptor, BelongsToUserInterceptor<TUser>>();
        }
    }
}
