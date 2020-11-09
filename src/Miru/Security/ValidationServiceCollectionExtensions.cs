using Microsoft.Extensions.DependencyInjection;

namespace Miru.Security
{
    public static class SecurityServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorizersInAssemblyOf<TAssemblyOfType>(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizer<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}