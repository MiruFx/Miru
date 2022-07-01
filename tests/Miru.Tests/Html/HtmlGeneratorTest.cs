using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html;

public class HtmlGeneratorTest
{
    private ITestFixture _;
    private readonly HtmlGenerator _htmlGenerator;

    public HtmlGeneratorTest()
    {
        _ = new ServiceCollection()
            .AddMiruHtml()
            .AddMiruCoreTesting()
            .BuildServiceProvider()
            .GetRequiredService<ITestFixture>();

        _htmlGenerator = _.Get<HtmlGenerator>();
    }

    [Test]
    public void Should_create_form_summary()
    {
        var model = new OrderNew.Command();
            
        var formSummary = _htmlGenerator.FormSummaryFor(model);

        formSummary.Id().ShouldBe("order-new-summary");
        formSummary.GetClasses().ShouldBeEmpty();
    }
        
    public class OrderNew
    {
        public class Command
        {
        }
    }
}