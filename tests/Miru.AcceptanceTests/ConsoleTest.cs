using System;
using System.Threading.Tasks;
using Miru.Core;
using Miru.Foundation.Hosting;
using Mong;
using NUnit.Framework;
using Shouldly;

namespace Miru.AcceptanceTests
{
    public class ConsoleTest
    {
        [Test]
        public async Task Can_run_config_show()
        {
            var defaultWriter = Console.Out;
            
            var writer = new StringWriter();
            Console.SetOut(writer);
            
            await MiruHost.CreateMiruHost<Startup>("miru", "config:show", "-e", "Test").RunMiruAsync();
            
            var output = writer.ToString();
            output.ShouldContain("EnvironmentName: Test");
            
            Console.SetOut(defaultWriter);
            Console.Write(writer.ToString());
        }
    }
}