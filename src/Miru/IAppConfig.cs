using Microsoft.Extensions.DependencyInjection;

namespace Miru
{
    public interface IAppConfig
    {
        void ConfigureServices(IServiceCollection services);
    }
}