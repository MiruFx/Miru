namespace Miru.PageTesting.Tests;

public class PageDisplayTest
{
    [TestFixture]
    public class PageDisplayTestChrome : Tests
    {
        public PageDisplayTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class PageDisplayTestFirefox : Tests
    {
        public PageDisplayTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }

    public abstract class Tests
    {
        protected DriverFixture _;

        [Test]
        public void Should_find_display()
        {
            _.HtmlIs(@"
<div id=""contact"">
    <h1>Home</h1>
    Name: <div id=""Name"">Paul</div>
    <div id=""Addresses[0].City"">Liverpool</div>
    <div id=""Addresses[1].City"">London</div>
</div>");

            _.Page.Display<Contact>(f =>
            {
                f.ShouldHaveText("Home");
                f.ShouldHaveText("Name:");
                f.ShouldHaveText("Paul");
            });
        }
        
        [Test]
        public void Should_scope_display_elements()
        {
            _.HtmlIs(@"
<h1>Home</h1>
<div id=""contact"">
    Name: <div id=""Name"">Paul</div>
</div>");

            _.Page.Display<Contact>(f =>
            {
                f.ShouldNotHaveText("Home");
                
                f.ShouldHaveText("Name:");
                f.ShouldHaveText("Paul");
            });
        }
        
        [Test]
        public void Should_find_display_labels()
        {
            _.HtmlIs(@"
<div id=""contact"">
    Name: <div id=""Name"">Paul</div>
    <div id=""Addresses[0].City"">Liverpool</div>
    <div id=""Addresses[1].City"">London</div>
</div>");

            _.Page.Display<Contact>(f =>
            {
                f.ShouldHave(m => m.Name, "Paul");
                f.ShouldHave(m => m.Addresses[0].City, "Liverpool");
                f.ShouldHave(m => m.Addresses[1].City, "London");
            });
        }
    }
}