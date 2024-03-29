using System;
using System.CommandLine;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Consolables;
using Miru.Hosting;
using StringWriter = System.IO.StringWriter;

namespace Miru.Tests.Consolables;

public class ConsolableTest
{
    private TextWriter _defaultConsoleWriter;
    private StringWriter _outWriter;
    private TextWriter _defaultConsoleError;
    private StringWriter _errorWriter;

    [SetUp]
    public void Setup()
    {
        _defaultConsoleWriter = Console.Out;
        _defaultConsoleError = Console.Error;
            
        _outWriter = new StringWriter();
        _errorWriter = new StringWriter();
            
        Console.SetOut(_outWriter);
        Console.SetError(_errorWriter);
    }

    [TearDown]
    public void Teardown()
    {
        Console.SetOut(_defaultConsoleWriter);
        Console.SetError(_defaultConsoleError);
    }

    [Test]
    public async Task Should_run_help()
    {
        // arrange
        var host = MiruHost.CreateMiruHost("miru")
            .ConfigureServices(x => x.AddConsolables<ConfigShowConsolable>());
                
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
            .ConfigureServices(x => x.AddConsolables<ConfigShowConsolable>());
                
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
            .ConfigureServices(x =>
            {
                x.AddScoped<IDependency, Dependency>();
                x.AddConsolable<ExampleConsolable>();
            });
            
        // act
        await host.RunMiruAsync();
            
        // assert
        var output = _outWriter.ToString();
        output.ShouldStartWith("Name: Paul, Surname: McCartney");
    }
        
    [Test]
    public void Should_scan_and_register_consolables_in_an_assembly()
    {
        // arrange
        var sp = new ServiceCollection()
            .AddConsolables<ConsolableTest>()
            .BuildServiceProvider();
            
        // act && assert
        sp.GetServices<Consolable>()
            .Any(x => x is ExampleConsolable)
            .ShouldBeTrue();
    }
        
    [Test]
    public async Task Should_not_throw_exception_when_option_is_not_defined()
    {
        // arrange
        var host = MiruHost.CreateMiruHost("miru", "config.show", "--Serilog:MinimumLevel:Override:Miru=Debug")
            .ConfigureServices(x => x.AddConsolables<ConfigShowConsolable>());
                
        // act
        await host.RunMiruAsync();
            
        // assert
        var errorOutput = _errorWriter.ToString();
            
        errorOutput.ShouldNotContain("Unrecognized command or argument");
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