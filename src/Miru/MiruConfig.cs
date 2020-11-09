using Microsoft.Extensions.DependencyInjection;

namespace Miru
{
    public abstract class MiruConfig
    {
        public abstract void ConfigureService(IServiceCollection services);
    }
}