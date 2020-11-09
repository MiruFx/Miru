using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases.EntityFramework;
using Miru.Foundation.Logging;
using Miru.Html;
using Miru.Mailing;
using Miru.Security;
using Miru.Userfy;
using Miru.Validation;

namespace Miru.Pipeline
{
    public static class PipelineServiceCollectionExtensions
    {
        public static IServiceCollection AddPipeline<TAssemblyOfType>(
            this IServiceCollection services, 
            Action<PipelineBuilder> builder) 
        {
            services.AddMediatR(typeof(TAssemblyOfType), typeof(EmailJob));
            
            var pipeline = new PipelineBuilder(services);
            builder.Invoke(pipeline);
            
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
                _.UseBehavior(typeof(SetUserBehavior<,>));
                _.UseBehavior(typeof(TransactionBehavior<,>));
                _.UseBehavior(typeof(AuthorizationBehavior<,>));
                _.UseBehavior(typeof(ValidationBehavior<,>));
            });
        }
    }
}
