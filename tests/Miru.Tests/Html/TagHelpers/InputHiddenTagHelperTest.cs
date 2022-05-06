using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class InputHiddenTagHelperTest : TagHelperTest
{
    [Test]
    public void Should_render_input_hidden_for_model()
    {
        // arrange
        var model = new Command { Name = "John Lennon" };
        var tag = CreateTag(new InputHiddenTagHelper(), model, m => m.Name);
            
        // act
        var html = ProcessTag(tag, "miru-hidden");
            
        // assert
        html.PreElement.GetContent().ShouldBe(
            "<input type=\"hidden\" value=\"John Lennon\" name=\"Name\" id=\"Name\">");
    }
       
    public class Command
    {
        public string Name { get; set; }
    }
}