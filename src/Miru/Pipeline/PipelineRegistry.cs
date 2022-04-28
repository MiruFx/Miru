using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases.EntityFramework;
using Miru.Foundation.Logging;
using Miru.Html;
using Miru.Mailing;
using Miru.Scopables;
using Miru.Scoping;
using Miru.Security;
using Miru.Userfy;
using Miru.Validation;

namespace Miru.Pipeline
{
    public static class PipelineServiceCollectionExtensions
    {
        public static IServiceCollection AddHandlers<TAssemblyOfType>(this IServiceCollection services) 
        {
            services.AddMediatR(typeof(TAssemblyOfType));
            services.AddValidators<TAssemblyOfType>();
            services.AddAuthorizersInAssemblyOf<TAssemblyOfType>();
            
            return services;
        }
        
        public static IServiceCollection AddPipeline<TAssemblyOfType>(
            this IServiceCollection services, 
            Action<PipelineBuilder> builder = null) 
        {
            // services.AddMediatR(typeof(TAssemblyOfType), typeof(EmailJob));
            services.AddMediatR(cfg =>
            {
                cfg.AsScoped();
                
            }, typeof(TAssemblyOfType), typeof(EmailJob));
            
            var pipeline = new PipelineBuilder(services);
            builder?.Invoke(pipeline);
            
            services.AddValidators<TAssemblyOfType>();
            services.AddAuthorizersInAssemblyOf<TAssemblyOfType>();
            services.AddScopes<TAssemblyOfType>();
            
            return services;
        }
        
        public static IServiceCollection AddScopes<TAssemblyOfType>(this IServiceCollection services)
        {
            services.AddScoped<MiruViewData>();
            
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IScopeFor<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            services.Scan(scan => scan
                .FromAssemblies(typeof(TAssemblyOfType).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IScope)))
                .AsSelf()
                .WithScopedLifetime());

            return services;
        }
        
        public static IServiceCollection AddDefaultPipeline<TAssemblyOfType>(this IServiceCollection services)
        {
            return services.AddPipeline<TAssemblyOfType>(_ =>
            {
                _.UseBehavior(typeof(LogBehavior<,>));
                _.UseBehavior(typeof(DumpRequestBehavior<,>));
                
                // should stop the pipeline if request is invalid
                _.UseBehavior(typeof(ValidationBehavior<,>));
                
                _.UseBehavior(typeof(TransactionBehavior<,>));
                _.UseBehavior(typeof(CurrentAttributesBehavior<,>));
                
                _.UseBehavior(typeof(AuthorizationBehavior<,>));
                _.UseBehavior(typeof(ScopeBehavior<,>));
            });
        }
    }
}
