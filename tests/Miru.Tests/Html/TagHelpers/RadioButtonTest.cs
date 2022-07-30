using Miru.Html;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class RadioButtonTest
{
    public class Boolean : MiruTagTesting
    {
        // [Test]
        // public async Task If_property_is_true_then_input_should_be_checked_and_value_true()
        // {
        //     // arrange
        //     var model = new Command { IsActive = true };
        //     var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
        //         
        //     // act
        //     var html = await ProcessTagAsync(tag, "miru-input");
        //         
        //     // assert
        //     html.HtmlShouldBe("<input type=\"radio\" value=\"True\" name=\"Interest\" id=\"Interest\" checked=\"checked\">");
        // }
        //     
        // [Test]
        // public async Task If_property_is_false_then_input_should_be_not_checked_and_value_false()
        // {
        //     // arrange
        //     var model = new Command { IsActive = false };
        //     var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.IsActive);
        //         
        //     // act
        //     var html = await ProcessTagAsync(tag, "miru-input");
        //         
        //     // assert
        //     html.HtmlShouldBe("<input type=\"radio\" value=\"False\" name=\"Interest\" id=\"Interest\">");
        // }
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