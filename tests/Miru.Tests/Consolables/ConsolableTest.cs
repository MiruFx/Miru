using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using Baseline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Config;
using Miru.Consolables;
using Miru.Core;
using Miru.Foundation.Hosting;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;
using StringWriter = System.IO.StringWriter;

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
            var host = MiruHost.CreateMiruHost("miru")
                .ConfigureServices(services =>
                {
                    services
                        .AddMiruCliHost()
                        .AddConsolables<ConfigShowConsolable>();
                });
                
            // act
            await host.RunMiruAsync();
            
            // assert
            var output = _outWriter.ToString();
            
            Console.WriteLine(output);

            output.ShouldContain("config.show");
            output.ShouldContain("Show the configuration will be used");
            
            output.ShouldContain("help");
        }

        [Test]
        public async Task Should_run_a_miru_command()
        {
            // arrange
            var host = MiruHost.CreateMiruHost("miru", "config.show")
                .ConfigureServices(services =>
                {
                    services
                        .AddMiruCliHost()
                        .AddConsolable<ConfigShowConsolable>();
                });
                
            // act
            await host.RunMiruAsync();
            
            // assert
            var output = _outWriter.ToString();
            
            output.ShouldContain("Miru version: ");
            output.ShouldContain("Host:");
            output.ShouldContain("EnvironmentName: Development");
            output.ShouldContain("All Configurations:");
        }

        [Test]
        public async Task Should_support_dependency_injection()
        {
            // arrange
            var host = MiruHost.CreateMiruHost("miru", "miru.dependency", "--name", "Paul")
                .ConfigureServices(services =>
                {
                    services.AddScoped<IDependency, Dependency>();
                    services.AddConsolable<ExampleConsolable>();
                });
            
            // act
            await host.RunMiruAsync();
            
            // assert
            var output = _outWriter.ToString();
            output.ShouldStartWith("Name: Paul, Surname: McCartney");
        }

        public interface IDependency
        {
            string Name { get; }
        }
        
        public class Dependency : IDependency
        {
            public string Name => "McCartney";
        }
        
        public class ExampleConsolable : Consolable
        {
            public ExampleConsolable() : base("miru.dependency")
            {
                Add(new Option<string>("--name"));
            }
            
            public class ConsolableHandler : IConsolableHandler
            {
                private readonly IDependency _dependency;

                public ConsolableHandler(IDependency dependency)
                {
                    _dependency = dependency;
                }

                public string Name { get; set; }
                
                public async Task Execute()
                {
                    Console.WriteLine($"Name: {Name}, Surname: {_dependency.Name}");
                    
                    await Task.CompletedTask;
                }
            }
        }
    }
}