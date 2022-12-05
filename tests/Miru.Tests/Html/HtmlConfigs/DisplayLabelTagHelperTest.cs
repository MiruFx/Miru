using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class DisplayLabelTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_display_label()
    {
        // arrange
        var model = new Command();
        var tag = TagWithFor(new DisplayLabelTagHelper(), model, m => m.Name);

        // act
        var output = ProcessTag(tag, "miru-display-label2");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<span>Name</span>");
    }
    
    // [Test]
    // public void If_display_label_has_content_then_should_render_the_content()
    // {
    //     // arrange
    //     var model = new Command();
    //     var tag = TagWithFor(new DisplayLabelTagHelper2(), model, m => m.Name);
    //
    //     // act
    //     var output = ProcessTag(tag, "miru-display-label2", content: "Your Full Name");
    //         
    //     // assert
    //     output.HtmlShouldBe(@"<span>Your Full Name</span>");
    // }
    
    public class Command
    {
        public string Name { get; set; }
    }
}