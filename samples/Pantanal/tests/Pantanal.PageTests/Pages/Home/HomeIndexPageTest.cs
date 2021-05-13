using Miru.PageTesting;
using NUnit.Framework;

namespace Pantanal.PageTests.Pages.Home
{
    public class HomeIndexPageTest : PageTest
    {
        [Test]
        public void Can_visit_index()
        {
            _.Visit("/");
        
            _.ShouldHaveText("Welcome to Pantanal");
        }
    }
}
