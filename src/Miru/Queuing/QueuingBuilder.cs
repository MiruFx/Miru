using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Queuing
{
    public class QueuingBuilder
    {
        public IServiceProvider ServiceProvider { get; }
        public IGlobalConfiguration Configuration { get; }

        public QueuingBuilder(
            IServiceProvider sp, 
            IGlobalConfiguration configuration,
            IServiceCollection services)
        {
            ServiceProvider = sp;
            Configuration = configuration;
        }
    }
}