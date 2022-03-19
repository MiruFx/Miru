namespace Miru.PageTesting.Tests;

public class NavigationTest
{
    [TestFixture]
    public class NavigationTestChrome : Tests
    {
        public NavigationTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class NavigationTestFirefox : Tests
    {
        public NavigationTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }

    public abstract class Tests
    {
        protected DriverFixture _;

        [Test]
        public void Can_navigate_to_a_path()
        {
            _.Page.NavigateTo("/products/list?order-by=name");
            
            _.Page.Url.ShouldEndWith("/products/list?order-by=name");
        }
    }
}