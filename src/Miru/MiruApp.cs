using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Miru
{
    public class MiruApp : IMiruApp
    {
        private readonly IServiceProvider _serviceProvider;

        public MiruApp(IServiceProvider serviceProvider)
        {
            App.ServiceProvider = serviceProvider;
            
            _serviceProvider = serviceProvider;
        }

        [DebuggerStepThrough]
        public T Get<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
        
        [DebuggerStepThrough]
        public T TryGet<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        
        [DebuggerStepThrough]
        public object Get(Type type)
        {
            return _serviceProvider.GetRequiredService(type);
        }

        [DebuggerStepThrough]
        public IEnumerable<T> GetAll<T>()
        {
            return _serviceProvider.GetServices<T>();
        }
        
        public void Dispose()
        {
        }
    }
}