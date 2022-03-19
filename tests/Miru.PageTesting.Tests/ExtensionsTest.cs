namespace Miru.PageTesting.Tests;

public class ExtensionsTest
{
    [TestFixture]
    public class ExtensionsTestChrome : Tests
    {
        public ExtensionsTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class ExtensionsTestFirefox : Tests
    {
        public ExtensionsTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }

    public abstract class Tests
    {
        protected DriverFixture _;
        
        [Test]
        public void Can_run_javascript()
        {
            _.HtmlIs(@"<h1>Title</h1>");
            
            _.Page.Nav.ConfigureExceptions(context =>
            {
                context.OriginalException.ShouldBeOfType<WebDriverTimeoutException>();
                context.Nav.ShouldBe(_.Page.Nav);
                context.FailureMessage.ShouldBe(@"Unable to find the text ""Sign In""");
                
                return new PageTestException("Custom exception message");
            });
            
            Should.Throw<PageTestException>(() => _.Page.ShouldHaveText("Sign In"))
                .Message
                .ShouldBe("Custom exception message");
        }
    }    
}