using Microsoft.Extensions.DependencyInjection;
using Miru.Settings;

namespace Miru.Tests.Hosting;

public class MiruHostTest
{
    [Test]
    public void Run_host_without_environment_should_be_development()
    {
        var sp = MiruHost.CreateMiruHost()
            .Build()
            .Services;
            
        sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Development");
    }
        
    [Test]
    public void Run_host_specifying_environment()
    {
        var sp = MiruHost.CreateMiruHost("--environment", "Staging")
            .Build()
            .Services;
            
        sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Staging");
    }
        
    [Test]
    public void Run_host_specifying_environment_shortcut()
    {
        var sp = MiruHost.CreateMiruHost("-e", "Test")
            .Build()
            .Services;
            
        sp.Get<IHostEnvironment>().EnvironmentName.ShouldBe("Test");
    }
        
    [Test]
    public void Run_host_with_configuration_from_command_line_args()
    {
        var sp = MiruHost
            .CreateMiruHost("--Database:ConnectionString=DataSource={{ db_dir }}App_dev.db")
            .Build()
            .Services;
            
        sp.Get<DatabaseOptions>().ConnectionString.ShouldBe("DataSource={{ db_dir }}App_dev.db");
    }
}