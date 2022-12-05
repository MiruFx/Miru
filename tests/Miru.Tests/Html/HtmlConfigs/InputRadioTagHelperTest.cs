using Miru.Html;
using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputRadioTagHelperTest : MiruTagTesting
{
    [Test]
    public void Input_type_radio_and_value_same_as_property_should_be_checked()
    {
        // arrange
        var model = new Command { Relationship = Relationships.Married };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Relationship);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = Relationships.Married });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""Married"" name=""Relationship"" id=""Relationship"" checked=""checked"" />");
    }
    
    [Test]
    public void Input_type_radio_and_value_int_same_as_property_enum_should_be_checked()
    {
        // arrange
        var model = new Command { Relationship = Relationships.Married };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Relationship);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = "2" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""2"" name=""Relationship"" id=""Relationship"" checked=""checked"" />");
    }
    
    [Test]
    public void Input_type_radio_and_value_different_from_property_should_not_be_be_checked()
    {
        // arrange
        var model = new Command { Relationship = Relationships.Married };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Relationship);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = Relationships.Divorced });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""Divorced"" name=""Relationship"" id=""Relationship"" />");       
    }
    
    [Test]
    public void Input_type_radio_with_value_and_property_null_should_not_be_checked()
    {
        // arrange
        var model = new Command { Size = null };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Size);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = "M" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""M"" name=""Size"" id=""Size"" />");
    }
    
    [Test]
    public void Input_type_radio_value_same_as_property_bool_should_checked()
    {
        // arrange
        var model = new Command { Active = false };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Active);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = "false" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""false"" name=""Active"" id=""Active"" checked=""checked"" />");
    }

    [Test]
    public void Input_type_radio_value_different_from_property_bool_should_not_be_checked()
    {
        // arrange
        var model = new Command { Active = true };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Active);
            
        // act
        var output = ProcessTag(tag, "miru-input2", new { type="radio", value = "false" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<input type=""radio"" value=""false"" name=""Active"" id=""Active"" />");
    }

    public class Command
    {
        [Radio]
        public Relationships Relationship { get; set; }
        public string Size { get; set; }
        public bool Active { get; set; }
    }

    public enum Relationships
    {
        Single = 1,
        Married = 2,
        Divorced = 3
    }
}