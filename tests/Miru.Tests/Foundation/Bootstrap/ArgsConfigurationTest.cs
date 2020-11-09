using Miru.Foundation.Bootstrap;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Foundation.Bootstrap
{
    public class ArgsConfigurationTest
    {
        [Test]
        public void Is_running_miru_cli()
        {
            var cfg = new ArgsConfiguration(new [] { "miru", "migrate:up" });
            
            cfg.RunCli.ShouldBeTrue();
            cfg.CliArgs.ShouldBe(new[] { "migrate:up" });
            
            new ArgsConfiguration(new [] { "miru" })
                .RunCli.ShouldBeTrue();
            
            new ArgsConfiguration(new [] { "other", "miru" })
                .RunCli.ShouldBeFalse();

            new ArgsConfiguration(new string[] { })
                .RunCli.ShouldBeFalse();
        }
        
        [Test]
        public void Read_environment()
        {
            new ArgsConfiguration(new [] { "miru", "migrate:up", "--environment", "integration" })
                .Environment.ShouldBe("integration");
            
            new ArgsConfiguration(new [] { "miru", "migrate:up", "--environment" })
                .Environment.ShouldBeNullOrEmpty();
            
            new ArgsConfiguration(new [] { "miru", "migrate:up", "-e", "Test" })
                .Environment.ShouldBe("Test");
            
            new ArgsConfiguration(new [] { "miru", "migrate:up", "-e" })
                .Environment.ShouldBeNullOrEmpty();
        }
        
        [Test]
        public void Read_verbose()
        {
            new ArgsConfiguration(new [] { "miru", "migrate:up", "--verbose" })
                .Verbose.ShouldBeTrue();
            
            new ArgsConfiguration(new [] { "miru", "migrate:up" })
                .Verbose.ShouldBeFalse();
        }
        
        [Test]
        public void All_options()
        {
            var cfg = new ArgsConfiguration(new [] { "miru", "migrate:up", "--verbose", "--environment", "Test" });
            cfg.Environment.ShouldBe("Test");
            cfg.Verbose.ShouldBeTrue();
            cfg.CliArgs.ShouldBe(new[] { "migrate:up" });
        }

        [Test]
        public void Remove_last_argument_if_is_project_assembly()
        {
            new ArgsConfiguration(new [] { "miru", "migrate:up", "D:/Projects/Miru/src/Shoppers/bin/Debug/netcoreapp2.2/Shoppers.dll" })
                .CliArgs.ShouldBe(new[] { "migrate:up" });
        }

        [Test]
        public void Should_exclude_verbose_from_miru_cli_args()
        {
            new ArgsConfiguration(new [] { "miru", "migrate:up", "--verbose" })
                .CliArgs.ShouldBe(new[] { "migrate:up" });
        }
        
        [Test]
        public void Should_exclude_environment_from_miru_cli_args()
        {
            new ArgsConfiguration(new [] { "miru", "migrate:up", "--environment", "Test" })
                .CliArgs.ShouldBe(new[] { "migrate:up" });
            
            new ArgsConfiguration(new [] { "miru", "migrate:up", "-e", "Test" })
                .CliArgs.ShouldBe(new[] { "migrate:up" });
        }
    }
}