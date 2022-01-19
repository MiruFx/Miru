using Miru.Mailing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests
{
    [TestFixture]
    public class LoggingExtensionTests
    {
        [Test]
        public void Should_inspect_empty_objects()
        {
            var model = new ProductList.Result();
            model.Inspect().ShouldBe(@"Empty");
        }
        
        [Test]
        public void Should_inspect_null_objects()
        {
            ProductList.Result model = null;
            model.Inspect().ShouldBe("null");
        }
        
        [Test]
        public void Should_inspect_objects()
        {
            var model = new ProductList.Query()
            {
                Category = "Apple",
                CategoryId = 123,
                OnSales = true
            };

            var output = model.Inspect();

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

            var output = model.Inspect().DumpToConsole();

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

            var output = model.Inspect().DumpToConsole();

            output.ShouldContain("Subject: Hello");
            output.ShouldNotContain("Body");
        }

        public class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}