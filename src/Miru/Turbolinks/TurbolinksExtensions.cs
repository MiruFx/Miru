using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Turbolinks
{
    public static class TurbolinksExtensions
    {
        public static IServiceCollection AddTurbolinks(this IServiceCollection services)
        {
            return services
                .AddSingleton<IActionResultExecutor<TurbolinksRedirectResult>, ContentResultExecutor>();
        }
    }
}