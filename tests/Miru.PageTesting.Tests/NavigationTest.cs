using NUnit.Framework;
using Shouldly;

namespace Miru.PageTesting.Tests
{
    public class NavigationTest
    {
        private readonly DriverFixture _ = DriverFixture.Get.Value.ForFirefox();

        [Test]
        public void Can_navigate_to_a_path()
        {
            _.Page.NavigateTo("/products/list?order-by=name");
            
            _.Page.Url.ShouldEndWith("/products/list?order-by=name");
        }
    }
}