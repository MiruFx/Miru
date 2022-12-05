using System.Collections.Generic;
using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class InputTextTagHelperTest : MiruTagTesting
{
    [Test]
    public void Input_for_and_property_string()
    {
        // arrange
        var model = new Command { Name = "John" };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Name);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""Name"" id=""Name"" type=""text"" value=""John"" />");
    }
    
    [Test]
    public void Input_for_and_property_int()
    {
        // arrange
        var model = new Command { Age = 88 };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Age);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""Age"" id=""Age"" type=""text"" value=""88"" />");
    }
    
    [Test]
    public void Input_for_and_property_children()
    {
        // arrange
        var model = new Command
        {
            Interests = new List<Interest>()
            {
                new(),
                new(),
                new()
                {
                    Attributes = new List<Attribute>
                    {
                        new() { Name = "Category" },
                        new() { Name = "Genre" },
                    }
                }
            }
        };
        var tag = TagWithFor(new InputTagHelper(), model, m => m.Interests[2].Attributes[1].Name);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""Interests[2].Attributes[1].Name"" id=""Interests_2__Attributes_1__Name"" type=""text"" value=""Genre"" />");
    }
    
    [Test]
    public void Input_model_and_attr_name_should_keep_name()
    {
        // arrange
        var model = new Command { Age = 88 };
        var tag = TagWithModel<InputTagHelper>(model.Age);

        // act
        var output = ProcessTag(tag, "miru-input2", new { name = "CustomerAge" });
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""CustomerAge"" id=""CustomerAge"" type=""text"" value=""88"" />");
    }

    [Test]
    public void Input_x_for_with_and_property_string()
    {
        // arrange
        var model = new Command { Name = "Paul" };
        var tag = TagWithXFor(new InputTagHelper(), model, x => x.Name);

        // act
        var output = ProcessTag(tag, "miru-input2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<input name=""Name"" id=""Name"" type=""text"" value=""Paul"" />");
    }

    public class Command
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Interest> Interests { get; set; } = new();
    }
        
    public class Interest
    {
        public List<Attribute> Attributes { get; set; } = new();
    }
        
    public class Attribute
    {
        public string Name { get; set; }
    }
}