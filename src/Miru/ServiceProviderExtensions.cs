using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Miru
{
    public static class ServiceProviderExtensions
    {
        public static IEnumerable<Type> GetRegisteredServices<TForType>(this IServiceProvider sp)
        {
            return sp.GetService<IServiceCollection>()
                .Where(_ => _.ServiceType == typeof(TForType))
                .Select(_ => _.ImplementationType)
                .ToImmutableList();
        }
        
        public static IEnumerable<Type> GetRegisteredServices<TForType>(this IMiruApp app)
        {
            return app.Get<IServiceCollection>()
                .Where(_ => _.ServiceType == typeof(TForType))
                .Select(_ => _.ImplementationType)
                .ToImmutableList();
        }
    }
}