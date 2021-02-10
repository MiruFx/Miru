using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Miru.Pipeline;

namespace Miru.Scoping
{
    public class ScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopeBehavior(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
        {
            var scopeFor = _serviceProvider.GetService<IScopeFor<TRequest>>();

            if (scopeFor == null)
                return await next();
            
            var scopeTypes = scopeFor.Scopes;

            foreach (var scopeType in scopeTypes)
            {
                IScope scope = (IScope) _serviceProvider.GetRequiredService(scopeType);

                await scope.Handle(request, ct);
            }

            var miruViewData = _serviceProvider.GetRequiredService<MiruViewData>();
            
            foreach (var accessor in scopeFor.Accessors)
            {
                var name = accessor.InnerProperty.Name;
                var instance = accessor.GetValue(request);
                
                miruViewData[name] = instance;
            }

            return await next();
        }
    }
}