using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class BootstrapHtmlConventionsTest : MiruTagTesting
{
    [Test]
    public void If_input_is_radio_then_should_have_()
    {
        // arrange
        var model = new Command { CustomerName = "Ringo" };
        var tag = TagWithXFor(new ValidationTagHelper(), model, m => m.CustomerName);

        // act
        var output = ProcessTag(tag, "miru-validation2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<div id=""CustomerName-validation"" data-for=""CustomerName"" hidden=""hidden""></div>");
    }
    
    [Test]
    public void Validation_for_and_flat_property()
    {
        // arrange
        var model = new Command { CustomerName = "Dawid" };
        var tag = TagWithFor(new ValidationTagHelper(), model, m => m.CustomerName);

        // act
        var output = ProcessTag(tag, "miru-validation2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<div id=""CustomerName-validation"" data-for=""CustomerName"" hidden=""hidden""></div>");
    }
    
    public class Command
    {
        public string CustomerName { get; set; }
    }
}