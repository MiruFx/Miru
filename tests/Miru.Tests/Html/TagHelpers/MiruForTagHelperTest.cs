using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class MiruForTagHelperTest : TagHelperTest
{
    [Test]
    public async Task Should_append_class_attribute()
    {
        // arrange
        var model = new Command { Name = "John" };
        var tag = CreateTagWithFor(new InputTagHelper
        {
            AddClass = "added-class"
        }, model, m => m.Name);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-input");
            
        // assert
        html.HtmlShouldBe(
            "<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\" class=\"added-class\">");            
    }
    
    [Test]
    public async Task Should_overwrite_existing_class_attribute()
    {
        // arrange
        var model = new Command { Name = "John" };
        var tag = CreateTagWithFor(new InputTagHelper() { SetClass = "existent-class" }, model, m => m.Name);
        
        // act
        var html = await ProcessTagAsync(tag, "miru-input", new { @class = "random-class" });
            
        // assert
        html.HtmlShouldBe(
            "<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\" class=\"existent-class\">");            
    }
    
    public class Command
    {
        public string Name { get; set; }
    }
}