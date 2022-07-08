using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class LabelTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task Should_render_label()
    {
        // arrange
        var command = new Command();
        var tag = CreateTagWithFor(new LabelTagHelper(), command, m => m.Name);
        
        // act
        var output = await ProcessTagAsync(tag, "miru-label");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<label for=\"Name\">Name</label>");
    }

    [Test] 
    public async Task Can_set_label_content()
    {
        // arrange
        var command = new Command();
        var tag = CreateTagWithFor(new LabelTagHelper(), command, m => m.Name);
        
        // act
        var output = await ProcessTagAsync(tag, "miru-label", "Customer Name");
            
        // assert
        output.TagName.ShouldBeNull();
        output.PreElement.GetContent().ShouldBe("<label for=\"Name\">Customer Name</label>");
    }
     
    public class Command
    {
        public string Name { get; set; }
    }
}