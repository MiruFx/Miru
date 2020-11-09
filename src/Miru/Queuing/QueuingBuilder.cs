using System;
using Hangfire;

namespace Miru.Queuing
{
    public class QueuingBuilder
    {
        public IServiceProvider ServiceProvider { get; }
        public IGlobalConfiguration Configuration { get; }

        public QueuingBuilder(IServiceProvider sp, IGlobalConfiguration configuration)
        {
            ServiceProvider = sp;
            Configuration = configuration;
        }
    }
}