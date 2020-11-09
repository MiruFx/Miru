using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Miru.Testing
{
    public interface ITestConfig
    {
        void ConfigureTestServices(IServiceCollection services);
        
        void ConfigureRun(TestRunConfig run);
        
        IHostBuilder GetHostBuilder();
    }
}