using Microsoft.Extensions.Logging;
using Miru.Hosting;

namespace Miru.Tests.Hosting;

[TestFixture]
public class MiruHostLoggingTest
{
    [Test]
    public void Should_set_miru_log_level_from_command_line()
    {
        var output = TestHelper.ReadingConsoleOutput(() =>
        {
            var sp = MiruHost.CreateMiruHost("--log", "Information")
                .Build()
                .Services;
            
            sp.Get<ILogger<App>>().LogInformation("Information!");
            sp.Get<ILogger<App>>().LogDebug("Debug!");
        });

        output.ShouldContain("Information!");
        output.ShouldNotContain("Debug!");
    }
}