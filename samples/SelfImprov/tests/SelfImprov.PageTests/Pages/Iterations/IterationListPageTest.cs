using Miru;
using Miru.PageTesting;
using NUnit.Framework;
using SelfImprov.Features.Iterations;
using Miru.Testing;
using Miru.Testing.Userfy;
using SelfImprov.Domain;

namespace SelfImprov.PageTests.Pages.Iterations
{
    public class IterationListPageTest : PageTest, IRequiresAuthenticatedUser
    {
        [Test]
        public void Can_list_iterations()
        {
            var iterations = _.MakeManySaving<Iteration>();
            
            _.Visit<IterationList>();
            
            _.ShouldHaveText("Iterations");

            _.Display<IterationList.Result>(x =>
            {
                x.ShouldHaveText(iterations.At(0).Number.ToString());
                x.ShouldHaveText(iterations.At(1).Number.ToString());
                x.ShouldHaveText(iterations.At(2).Number.ToString());
            });
        }
    }
}
