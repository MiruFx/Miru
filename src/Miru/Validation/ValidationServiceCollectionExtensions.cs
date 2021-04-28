using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Validation
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators<TAssemblyOfType>(this IServiceCollection services)
        {
            ValidatorOptions.Global.DisplayNameResolver = (_, memberInfo, _) => 
                memberInfo.GetCustomAttribute<DisplayAttribute>()?.GetName();
            
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}