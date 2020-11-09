using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Mailing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Mailing
{
    public class LiquidRendererTest
    {
        private readonly LiquidRenderer _renderer;

        public LiquidRendererTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<LiquidRenderer>()
                .AddSingleton<MiruSolution, MiruTestSolution>()
                .BuildServiceProvider();

            _renderer = serviceProvider.GetService<LiquidRenderer>();
        }

        [Test]
        public void Render_liquid_template()
        {
            var user = new User { Name = "John" };
            
            var result = _renderer.Render(A.Path("Mailing") / "Welcome.md", user);

            result.ShouldBe("# Welcome, John!");
        }

        public class User
        {
            public string Name { get; set; }
        }
    }
}