using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Miru.Foundation.Hosting;
using Miru.Queuing;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Consolables
{
    public class ConsolableTest
    {
        private TextWriter _defaultConsoleWriter;
        private StringWriter _outWriter;

        [SetUp]
        public void Setup()
        {
            _defaultConsoleWriter = Console.Out;
            _outWriter = new StringWriter();
            
            Console.SetOut(_outWriter);
        }

        [TearDown]
        public void Teardown()
        {
            Console.SetOut(_defaultConsoleWriter);
        }

        [Test]
        public async Task Should_run_help()
        {
            // arrange
            var host = MiruHost.CreateMiruHost("miru");
                
            // act
            await host.RunMiruAsync();
            
            // assert
            var output = _outWriter.ToString();
            
            output.ShouldContain("config:show       Show the configuration will be used");
            output.ShouldContain("help              list all the available commands");
        }

        [Test]
        public async Task Should_run_a_miru_command()
        {
            // arrange
            var host = MiruHost.CreateMiruHost("miru", "config:show");
                
            // act
            await host.RunMiruAsync();
            
            // assert
            var output = _outWriter.ToString();
            
            output.ShouldContain("Miru version: ");
            output.ShouldContain("Host:");
            output.ShouldContain("EnvironmentName: Development");
            output.ShouldContain("All Configurations:");
        }
    }
}