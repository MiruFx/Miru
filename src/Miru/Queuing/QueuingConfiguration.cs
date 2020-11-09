using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Storages;

namespace Miru.Queuing
{
    public abstract class QueuingConfiguration : MiruConfig
    {
        public string LocalStorage(IServiceProvider sp) => 
            sp.GetService<Storage>().MakePath() / "db" / $"Queue_{sp.GetService<IHostEnvironment>().EnvironmentName}.db";
    }
}