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
        var output = ReadingConsoleOutput(() =>
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
    
    public static string ReadingConsoleOutput(Action action)
    {
        var defaultWriter = Console.Out;
        var writer = new StringWriter();
            
        Console.SetOut(writer);
            
        action();
            
        var output = writer.ToString();
        Console.SetOut(defaultWriter);

        return output;
    }
    
    public class StringWriter : TextWriter
    {
        private readonly StringBuilder _content = new StringBuilder();

        public override void Write(char value)
        {
            _content.Append(value);
        }

        public override void Write(string value)
        {
            _content.Append(value);
        }

        public override string ToString() => _content.ToString();

        public override Encoding Encoding => Encoding.Unicode;
    }
}
    
