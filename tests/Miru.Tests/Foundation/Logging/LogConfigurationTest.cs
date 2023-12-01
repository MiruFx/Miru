using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miru.Foundation.Logging;
using Serilog;

namespace Miru.Tests.Foundation.Logging;

public class LogConfigurationTest
{
    [Test]
    public void Can_accumulate_overwriting_serilog_configuration()
    {
        var output = TestHelper.ReadingConsoleOutput(() =>
        {
            var sp = new HostBuilder()
                .ConfigureSerilog(config => config.WriteTo.Console().MinimumLevel.Fatal())
                .ConfigureSerilog(config => config.MinimumLevel.Debug())
                .Build()
                .Services;

            sp.GetService<ILogger<LogConfigurationTest>>().LogDebug("Debug!");
            sp.GetService<ILogger<LogConfigurationTest>>().LogInformation("Information!");
        });
            
        output.ShouldContain("Debug!");
        output.ShouldContain("Information!");
    }
}
    
