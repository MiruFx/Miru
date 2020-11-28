using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Fabrication;
using Miru.Foundation.Hosting;
using Miru.Foundation.Logging;
using Miru.Queuing;
using Miru.Sqlite;
using Miru.Testing;
using Miru.Testing.Userfy;
using Mong.Domain;
using Mong.Payments;
using Serilog.Events;

namespace Mong.Tests.Config
{
    public class TestsConfig : ITestConfig
    {
        public virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddFeatureTesting()
                .AddSqliteDatabaseCleaner()
                .AddQueueCleaner<LiteDbQueueCleaner>()
                .AddFabrication<MongFabricator>();

            services.Mock<IPayPau>();
            services.Mock<IMobileProvider>();

            services.AddSerilogConfig(cfg =>
            {
                cfg.Testing(LogEventLevel.Debug);
            });
        }
    
        public void ConfigureRun(TestRunConfig run)
        {
            // This run configurations only works for tests that inherits MiruTest
            // It includes FeatureTest, PageTest and all other Miru's types of tests
            
            run.TestingDefault();

            run.UserfyRequiresAdmin<User>();
        }
        
        public virtual IHostBuilder GetHostBuilder()
        {
            return MiruHost.CreateMiruHost<Startup>();
        }
    }
}