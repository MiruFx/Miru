using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class ValidationMessageTagHelperTest : MiruTagTesting
{
    [Test]
    public void Validation_x_for_and_flat_property()
    {
        // arrange
        var model = new Command { CustomerName = "Ringo" };
        var tag = TagWithXFor(new ValidationTagHelper(), model, m => m.CustomerName);

        // act
        var output = ProcessTag(tag, "miru-validation2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<div id=""CustomerName-validation"" data-for=""CustomerName"" hidden=""hidden""></div>");
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
        Helpers.Extensions.HtmlShouldBe(output, @"<div id=""CustomerName-validation"" data-for=""CustomerName"" hidden=""hidden""></div>");
    }
    
    public class Command
    {
        public string CustomerName { get; set; }
    }
}