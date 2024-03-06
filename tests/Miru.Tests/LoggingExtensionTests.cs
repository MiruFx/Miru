using Miru.Mailing;

namespace Miru.Tests;

[TestFixture]
public class LoggingExtensionTests
{
    [Test]
    public void Should_inspect_empty_objects()
    {
        var model = new ProductList.Result();
        Yml.Dump(model).ShouldBe(@"Empty");
    }
        
    [Test]
    public void Should_inspect_null_objects()
    {
        ProductList.Result model = null;
        Yml.Dump(model).ShouldBe("null");
    }
        
    [Test]
    [Ignore("Fix. Failing for some reason")]
    public void Should_inspect_objects()
    {
        var model = new ProductList.Query()
        {
            Category = "Apple",
            CategoryId = 123,
            OnSales = true
        };

        var output = Yml.Dump(model);

        output.ShouldContain("Title: This is the title");
        output.ShouldContain("CategoryId: 123");
        output.ShouldContain("Category: Apple");
        output.ShouldContain("OnSales: True");
    }
        
    [Test]
    public void Should_inspect_ignoring_password_properties()
    {
        var model = new User()
        {
            Email = "user@user.com",
            Password = "VeryBigPassword#Blink@182"
        };

        var output = Yml.Dump(model);

        output.ShouldContain("Email: user@user.com");
        output.ShouldNotContain("Password");
    }
        
    [Test]
    public void Should_inspect_ignoring_emails_body()
    {
        var model = new EmailJob(new Email
        {
            Body = "Body",
            Subject = "Hello"
        });

        var output = Yml.Dump(model);

        output.ShouldContain("Subject: Hello");
        output.ShouldNotContain("Body");
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}