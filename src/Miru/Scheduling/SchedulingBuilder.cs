using System;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Scheduling
{
    public class SchedulingBuilder
    {
        public IServiceProvider ServiceProvider { get; }
        
        public SchedulingBuilder(
            IServiceProvider sp, 
            IServiceCollection services)
        {
            ServiceProvider = sp;
        }
    }
}