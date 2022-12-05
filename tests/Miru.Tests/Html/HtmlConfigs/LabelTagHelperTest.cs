using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class LabelTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_label()
    {
        // arrange
        var model = new Command();
        var tag = TagWithFor(new LabelTagHelper(), model, m => m.Name);

        // act
        var output = ProcessTag(tag, "miru-label2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<label for=""Name"">Name</label>");
    }
    
    [Test]
    public void If_label_has_content_then_should_render_the_content()
    {
        // arrange
        var model = new Command();
        var tag = TagWithFor(new LabelTagHelper(), model, m => m.Name);

        // act
        var output = ProcessTag(tag, "miru-label2", content: "Your Full Name");
            
        // assert
        // in production, asp.net will set the html's content
        Helpers.Extensions.HtmlShouldBe(output, @"<label for=""Name""></label>");
    }
    
    // [Test]
    // public void If_content_has_been_set_and_has_no_html_content_then_should_render_set_content()
    // {
    //     // arrange
    //     var model = new Command();
    //     var tag = TagWithFor(new LabelTagHelper2(), model, m => m.Name);
    //     
    //     // act
    //     var output = ProcessTag(tag, "miru-label2", content: "Your Full Name");
    //         
    //     // assert
    //     output.HtmlShouldBe(@"<label for=""Name"">Your Full Name</label>");
    // }
    
    public class Command
    {
        public string Name { get; set; }
    }
}