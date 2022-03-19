namespace Miru.PageTesting.Tests;

public class ExceptionTest
{
    [TestFixture]
    public class ExceptionTestChrome : Tests
    {
        public ExceptionTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class ExceptionTestFirefox : Tests
    {
        public ExceptionTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }

    public abstract class Tests
    {
        protected DriverFixture _;
        
        [Test]
        public void Can_configure_action_when_exception_occurs()
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