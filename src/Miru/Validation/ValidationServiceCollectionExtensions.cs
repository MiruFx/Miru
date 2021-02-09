using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Validation
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators<TAssemblyOfType>(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}