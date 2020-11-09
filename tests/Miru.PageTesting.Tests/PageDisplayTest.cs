using System.Collections.Generic;
using NUnit.Framework;

namespace Miru.PageTesting.Tests
{
    public class PageDisplayTest
    {
        private readonly DriverFixture _ = DriverFixture.Get.Value.ForFirefox();

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

    public class Contact
    {
        public string Name { get; set; }
        public List<Address> Addresses { get; } = new List<Address>();
    }

    public class Address
    {
        public string City { get; set; }
    }
}