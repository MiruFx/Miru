namespace Miru.PageTesting.Tests;

public class ShouldTest
{
    [TestFixture]
    public class ShouldTestChrome : Tests
    {
        public ShouldTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class ShouldTestFirefox : Tests
    {
        public ShouldTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }

    public abstract class Tests
    {
        protected DriverFixture _;

        [Test]
        public void Should_have_text()
        {
            _.HtmlIs("<h1>Orders</h1><h3>List of your last orders</h3>");

            _.Page.ShouldHaveText("last orders");
        }
        
        [Test]
        public void Should_not_have_text()
        {
            _.HtmlIs("<h1>Orders</h1><h3>List of your last orders</h3>");

            _.Page.ShouldNotHaveText("last products");
        }
        
        [Test]
        public void Throw_exception_if_should_have_text_not_match()
        {
            _.HtmlIs("<h1>Orders</h1><h3>List of your last orders</h3>");

            Should.Throw<PageTestException>(() => _.Page.ShouldHaveText("last meals"))
                .Message
                .ShouldContain(@"Unable to find the text ""last meals""");
        }
        
        [Test]
        public void Should_have_title()
        {
            _.HtmlIs(@"
<h1>Orders</h1>
<h2>Primary Orders</h2>
<h3>Last Orders</h3>
<h4>Pending Orders</h4>
<h5>Not Paid</h5>
<h6>Primary Last Pending Not Paid Orders</h6>");

            _.Page.ShouldHaveTitle("Orders");
            _.Page.ShouldHaveTitle("Primary Orders");
            _.Page.ShouldHaveTitle("Last Orders");
            _.Page.ShouldHaveTitle("Pending Orders");
            _.Page.ShouldHaveTitle("Not Paid");
            _.Page.ShouldHaveTitle("Primary Last Pending Not Paid Orders");
        }
        
        [Test]
        public void Throw_exception_if_should_have_title_not_match()
        {
            _.HtmlIs(@"
<h1>Orders</h1>
<h2>Primary Orders</h2>
<h3>Last Orders</h3>");

            Should.Throw<ElementNotFoundException>(() => _.Page.ShouldHaveTitle("Primary"))
                .Message
                .ShouldContain(@"Unable to find the title ""Primary""");
        }
    }
}