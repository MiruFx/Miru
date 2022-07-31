using Miru.Html;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

[TestFixture]
public class RadioButtonTest
{
    public class Boolean : MiruTagTesting
    {
        [Test]
        public async Task If_input_is_for_false_and_property_is_false_then_input_should_be_checked()
        {
            // arrange
            var model = new Command { IsActive = false };
            var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
                
            // act
            var html = await ProcessTagAsync(tag, "miru-input", new { value = "False" });
                
            // assert
            html.HtmlShouldBe("<input type=\"radio\" value=\"False\" name=\"IsActive\" id=\"IsActive\" checked=\"checked\">");
        }
            
        [Test]
        public async Task If_input_is_for_false_and_property_is_true_then_input_should_be_not_checked()
        {
            // arrange
            var model = new Command { IsActive = true };
            var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
                
            // act
            var html = await ProcessTagAsync(tag, "miru-input", new { value = "False" });
                
            // assert
            html.HtmlShouldBe("<input type=\"radio\" value=\"False\" name=\"IsActive\" id=\"IsActive\">");
        }
        
        [Test]
        public async Task If_input_is_for_true_and_property_is_true_then_input_should_be_checked()
        {
            // arrange
            var model = new Command { IsActive = true };
            var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
                
            // act
            var html = await ProcessTagAsync(tag, "miru-input", new { value = "True" });
                
            // assert
            html.HtmlShouldBe("<input type=\"radio\" value=\"True\" name=\"IsActive\" id=\"IsActive\" checked=\"checked\">");
        }
        
        [Test]
        public async Task If_input_is_for_true_and_property_is_false_then_input_should_be_not_checked()
        {
            // arrange
            var model = new Command { IsActive = false };
            var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
                
            // act
            var html = await ProcessTagAsync(tag, "miru-input", new { value = "True" });
                
            // assert
            html.HtmlShouldBe("<input type=\"radio\" value=\"True\" name=\"IsActive\" id=\"IsActive\">");
        }
    }
    
    public class Command
    {
        [Radio]
        public Interest Interest { get; set; }
        
        [Radio]
        public bool IsActive { get; set; }
    }
        
    public enum Interest
    {
        Yes,
        No,
        Maybe
    }
}