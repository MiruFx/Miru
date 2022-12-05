using System.Collections.Generic;
using Miru.Html;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputCheckboxTagHelperTest : MiruTagTesting
{
    [Test]
    public void Input_type_checkbox_for_and_property_boolean_false()
    {
        // arrange
        var model = new Command { Remember = false };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "checkbox" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<input type=\"checkbox\" name=\"Remember\" id=\"Remember\" value=\"true\" /><input name=\"Remember\" type=\"hidden\" value=\"false\" />");
    }
        
    [Test]
    public void Input_for_and_property_boolean_true_should_be_checked()
    {
        // arrange
        var model = new Command { Remember = true };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "checkbox" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, "<input type=\"checkbox\" name=\"Remember\" id=\"Remember\" value=\"true\" checked=\"checked\" /><input name=\"Remember\" type=\"hidden\" value=\"false\" />");
    }
        
    [Test]
    public void Input_checked_true_and_property_boolean_false_should_be_checked()
    {
        // arrange
        var model = new Command { Remember = false };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type = "checkbox", @checked = "checked" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input type=""checkbox"" checked=""checked"" name=""Remember"" id=""Remember"" value=""true"" /><input name=""Remember"" type=""hidden"" value=""false"" />");
    }
 
    [Test]
    [Ignore("check where is being used")]
    public void Input_value_same_as_model_and_property_enum_should_be_checked()
    {
        // arrange
        var model = new Command { NewsletterOptions = new[] { Newsletters.Offers, Newsletters.Partners }};
        var tag = TagWithFor(new InputTagHelper(), model, m => m.NewsletterOptions);

        // act
        var output = ProcessTag(tag, "miru-input2", new { value = Newsletters.Partners });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input value=""Partners"" name=""NewsletterOptions"" id=""NewsletterOptions"" type=""checkbox"" checked=""checked"" />");
    }
    
    [Test]
    [Ignore("check where is being used")]
    public void Input_value_different_from_model_and_property_enum_should_not_be_checked()
    {
        // arrange
        var model = new Command { NewsletterOptions = new[] { Newsletters.Offers, Newsletters.Partners }};
        var tag = TagWithFor(new InputTagHelper(), model, m => m.NewsletterOptions);

        // act
        var output = ProcessTag(tag, "miru-input2", new { value = Newsletters.News });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input value=""News"" name=""NewsletterOptions"" id=""NewsletterOptions"" type=""checkbox"" />");
    }
        
    // [Test]
    // public void Input_x_for_type_checkbox_and_model_property_string_should_be_checked()
    // {
    //     // arrange
    //     var model = new Command { Value = "Green" };
    //     var tag = TagWithXFor(new InputTagHelper2(), model, x => x.Value);
    //
    //     // act
    //     var output = ProcessTag(tag, "miru-input2", new { type = "checkbox" });
    //         
    //     // assert
    //     output.HtmlShouldBe(@"<input type=""checkbox"" name=""Value"" id=""Value"" value=""Green"" checked=""checked"" />");
    // }
    //
    // [Test]
    // public void Input_x_for_type_checkbox_and_value_with_model_property_string_should_be_checked()
    // {
    //     // arrange
    //     var model = new Command { Value = "Red" };
    //     var tag = TagWithXFor(new InputTagHelper2(), model, x => x.Value);
    //
    //     // act
    //     var output = ProcessTag(tag, "miru-input2", new { type = "checkbox", value = "Red" });
    //         
    //     // assert
    //     output.HtmlShouldBe(@"<input type=""checkbox"" value=""Red"" name=""Value"" id=""Value"" checked=""checked"" />");
    // }
    
    public class Command
    {
        public string Value { get; set; }
        public bool Remember { get; set; }
        [Checkbox]
        public IEnumerable<Newsletters> NewsletterOptions { get; set; }
    }
        
    public enum Newsletters
    {
        Offers,
        News,
        Partners
    }
}