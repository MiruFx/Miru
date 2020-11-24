using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Consolables;
using Miru.Databases.Migrations;
using Miru.Foundation.Hosting;
using Miru.Makers;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Consolables
{
    public class ConsolableServiceCollectionExtensionsTest
    {
        [Test]
        public void Can_add_consolables_from_an_assembly()
        {
            var consolableTypes = new ServiceCollection()
                .AddServiceCollection()
                .AddConsolables<ConsolableServiceCollectionExtensionsTest>() // act
                .BuildServiceProvider()
                .GetRegisteredServices<IConsolable>();
            
            consolableTypes.ShouldContain(m => m == typeof(TestConsolable));
        }

        [Test]
        public void Can_add_a_consolable()
        {
            var consolableTypes = new ServiceCollection()
                .AddServiceCollection()
                .AddConsolable<TestConsolable>() // act
                .BuildServiceProvider()
                .GetRegisteredServices<IConsolable>();
            
            consolableTypes.ShouldContain(m => m == typeof(TestConsolable));
        }
        
        [Test]
        public void Miru_host_should_have_miru_default_consolables()
        {
            // act
            var consolableTypes = MiruHost
                .CreateMiruHost()
                .Build()
                .Services
                .GetRegisteredServices<IConsolable>();
                
            // assert
            consolableTypes.ShouldContain(m => m == typeof(ConfigShowConsolable));
            consolableTypes.ShouldContain(m => m == typeof(MakeConsolableConsolable));
        }

        public class TestConsolable : ConsolableSync 
        {
            public override void Execute()
            {
            }
        }
    }
}