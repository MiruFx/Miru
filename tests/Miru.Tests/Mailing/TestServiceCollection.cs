using System;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Tests.Mailing
{
    public class TestServiceCollection
    {
        public ServiceCollection Services { get; private set; }
        
        public IServiceProvider ServiceProvider { get; private set; }
        
        public static TestServiceCollection Build(Action<IServiceCollection> services)
        {
            var test = new TestServiceCollection
            {
                Services = new ServiceCollection()
            };

            services(test.Services);

            test.ServiceProvider = test.Services.BuildServiceProvider();

            return test;
        }
    }
}