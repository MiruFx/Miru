using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Config;
using Miru.Core;
using Miru.Settings;

namespace Miru.AcceptanceTests
{
    public class TestConfig : Mong.Tests.Config.TestsConfig
    {
        private MiruSolution Solution { get; } = new SolutionFinder()
            .FromDir(Path.GetFullPath(MiruPath.CurrentPath / ".." / ".." / ".." / ".." / ".." / "samples" / "Mong")).Solution;

        public override IHostBuilder GetHostBuilder()
        {
            return base.GetHostBuilder()
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    cfg.AddConfigYml(Solution.ConfigDir, hostingContext.HostingEnvironment.EnvironmentName);
                });
        }

        public override void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureTestServices(services);

            services.AddSingleton(new DatabaseOptions()
            {
                ConnectionString = $"DataSource={Solution.StorageDir / "db" / "Mong_test.db"}"
            });
            
            services.AddSingleton(Solution);                
        }
    }
}