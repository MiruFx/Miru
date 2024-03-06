using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Currentable;
using Miru.Databases.EntityFramework;
using Miru.Foundation.Logging;
using Miru.Globalization;
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
            _.UseBehavior(typeof(GlobalizationBehavior<,>));
            _.UseBehavior(typeof(LogBehavior<,>));
                
            // should stop the pipeline if request is invalid
            _.UseBehavior(typeof(ValidationBehavior<,>));
                
            _.UseBehavior(typeof(TransactionBehavior<,>));
            _.UseBehavior(typeof(CurrentBehavior<,>));
                
            _.UseBehavior(typeof(AuthorizationBehavior<,>));
        });
        
        // x.UseBehavior(typeof(GlobalizationBehavior<,>));
        // x.UseBehavior(typeof(LogBehavior<,>));
        // x.UseBehavior(typeof(ExceptionalBehavior<,>));
        // x.UseBehavior(typeof(Framework.TransactionBehavior<,>));
        // x.UseBehavior(typeof(CurrentBehavior<,>));
        // x.UseBehavior(typeof(ScopableBehavior<,>));
        // x.UseBehavior(typeof(ValidationBehavior<,>));
        // x.UseBehavior(typeof(AuthorizationBehavior<,>));
    }
}