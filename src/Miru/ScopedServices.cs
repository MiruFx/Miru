using System;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;

namespace Miru
{
    public class ScopedServices : IDisposable
    {
        private readonly IServiceScope _scope;
        private readonly IServiceProvider _services;

        public ScopedServices(IMiruApp app)
        {
            _scope = app.Get<IServiceProvider>().CreateScope();
            _services = _scope.ServiceProvider;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public T Get<T>()
        {
            return _services.GetService<T>();
        }
        
        public object Get(Type type)
        {
            return _services.GetService(type);
        }
    }
}