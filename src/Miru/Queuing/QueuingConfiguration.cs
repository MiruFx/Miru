using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Storages;

namespace Miru.Queuing
{
    public abstract class QueuingConfiguration
    {
        public string LocalStorage(IServiceProvider sp) => 
            sp.GetRequiredService<IStorage>().App / "db" / $"Queue_{sp.GetRequiredService<IHostEnvironment>().EnvironmentName}.db";
    }
}