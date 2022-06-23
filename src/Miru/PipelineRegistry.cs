using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases.EntityFramework;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Pipeline;
using Miru.Scopables;
using Miru.Security;
using Miru.Validation;

namespace Miru;

public static class PipelineRegistry
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
        services.AddMediatR(cfg =>
        {
            cfg.AsScoped();
                
        }, typeof(TAssemblyOfType), typeof(EmailJob));
            
        var pipeline = new PipelineBuilder(services);
        builder?.Invoke(pipeline);
            
        services.AddValidators<TAssemblyOfType>();
        services.AddAuthorizersInAssemblyOf<TAssemblyOfType>();
            
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
        });
    }
}