using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Foundation.Hosting;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Hosting
{
    public class MiruHostTest
    {
        [Test]
        public void Run_host_without_environment_should_be_development()
        {
            var sp = MiruHost.CreateMiruHost().Build().Services;
            
            sp.GetService<IHostEnvironment>().EnvironmentName.ShouldBe("Development");
        }
        
        [Test]
        public void Run_host_specifying_environment()
        {
            var sp = MiruHost.CreateMiruHost("--environment", "Staging").Build().Services;
            
            sp.GetService<IHostEnvironment>().EnvironmentName.ShouldBe("Staging");
        }
        
        [Test]
        public void Run_host_specifying_environment_shortcut()
        {
            var sp = MiruHost.CreateMiruHost("-e", "Test").Build().Services;
            
            sp.GetService<IHostEnvironment>().EnvironmentName.ShouldBe("Test");
        }
    }
}