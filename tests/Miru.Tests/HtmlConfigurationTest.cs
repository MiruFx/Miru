using Microsoft.Extensions.DependencyInjection;
using Miru.Html;

namespace Miru.Tests;

public class HtmlRegistryTest
{
    [Test]
    public void Should_add_miru_html_services()
    {
        // arrange
        var htmlGenerator = new ServiceCollection()
            .AddMiruHtml<TestHtmlConfig>()
            .BuildServiceProvider()
            .GetRequiredService<HtmlGenerator>();
        
        // act
        var tag = htmlGenerator.TableFor(new object());

        // assert
        tag.Text().ShouldBe("Hello!");
    }

    [Test]
    public void If_added_new_html_config_then_should_replace_previous_one()
    {
        // arrange
        var htmlGenerator = new ServiceCollection()
            .AddMiruHtml<TestHtmlConfig>()
            .AddMiruHtml<AnotherTestHtmlConfig>()
            .BuildServiceProvider()
            .GetRequiredService<HtmlGenerator>();
        
        // act
        var tag = htmlGenerator.TableFor(new object());

        // assert
        tag.Text().ShouldBe("Another Hello!");
    }
    
    [Test]
    public void If_html_config_not_specified_when_registering_miru_then_should_add_default_config()
    {
        var htmlGenerator = new ServiceCollection()
            .AddMiru<HtmlRegistryTest>()
            .BuildServiceProvider()
            .GetRequiredService<HtmlGenerator>();
        
        // act
        var tag = htmlGenerator.TableFor(new object());

        // assert
        tag.Text().ShouldBeEmpty();
    }

    public class TestHtmlConfig : HtmlConfiguration
    {
        public TestHtmlConfig()
        {
            Tables.Always.ModifyTag(tag => tag.Text("Hello!"));
        }
    }
    
    public class AnotherTestHtmlConfig : HtmlConfiguration
    {
        public AnotherTestHtmlConfig()
        {
            Tables.Always.ModifyTag(tag => tag.Text("Another Hello!"));
        }
    }
}