using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Miru.Core;
using Miru.Foundation.Logging;
using NUnit.Framework;
using Serilog;
using Shouldly;

namespace Miru.Tests.Foundation.Logging
{
    public class LogConfigurationTest
    {
        [Test]
        public void Can_accumulate_overwriting_serilog_configuration()
        {
            var output = Console2.ReadingConsoleOutput(() =>
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
}