using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Miru.Config;
using Miru.Consolables;
using Miru.Databases.Migrations;
using Miru.Makers;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Consolables
{
    public class ConsolableServiceCollectionExtensionsTest
    {
        [Test]
        public void Can_add_a_consolable()
        {
            var consolableTypes = new ServiceCollection()
                .AddServiceCollection()
                .AddConsolable<TestConsolable>() // act
                .BuildServiceProvider()
                .GetRegisteredServices<Consolable>();
            
            consolableTypes.ShouldContain(m => m == typeof(TestConsolable));
        }
        
        public class TestConsolable : Consolable
        {
            public TestConsolable() : base("consolable.test")
            {
            }
            
            public class ConsolableHandler : IConsolableHandler
            {
                public Task Execute()
                {
                    return Task.CompletedTask;
                }
            }
        }
    }
}