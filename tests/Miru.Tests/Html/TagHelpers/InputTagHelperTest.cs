using System.Collections.Generic;
using Miru.Html;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class InputTagHelperTest : TagHelperTest
    {
        [Test]
        public void When_input_is_radio_and_property_enum_should_set_selected_if_model_is_equal_radio_value()
        {
            // arrange
            var model = new Command { Relationship = Relationships.Divorced };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Relationship);
            
            // act
            var htmlChecked = ProcessTag(tag, "miru-input", new { value = model.Relationship });
            var htmlNotChecked = ProcessTag(tag, "miru-input", new { value = Relationships.Married });
            
            // assert
            htmlChecked.PreElement.GetContent().ShouldBe(
                "<input type=\"radio\" value=\"Divorced\" name=\"Relationship\" id=\"Relationship\" checked=\"checked\">");
            
            htmlNotChecked.PreElement.GetContent().ShouldBe(
                "<input type=\"radio\" value=\"Married\" name=\"Relationship\" id=\"Relationship\">");            
        }
       
        [Test]
        public void Should_render_text_input()
        {
            // arrange
            var model = new Command { Name = "John" };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Name);

            // act
            var html = ProcessTag(tag, "miru-input");
            
            // assert
            html.PreElement.GetContent().ShouldBe("<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\">");
        }
        
        [Test]
        public void Should_render_text_input_for_nested_list_property()
        {
            // arrange
            var model = new Command
            {
                Interests = new List<Interest>()
                {
                    new Interest(),
                    new Interest(),
                    new Interest()
                    {
                        Attributes = new List<Attribute>()
                        {
                            new Attribute() { Name = "Category" },
                            new Attribute() { Name = "Genre" },
                        }
                    }
                }
            };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Interests[2].Attributes[1].Name);

            // act
            var html = ProcessTag(tag, "miru-input");
            
            // assert
            html.PreElement.GetContent().ShouldBe("<input type=\"text\" value=\"Genre\" name=\"Interests[2].Attributes[1].Name\" id=\"Interests_2__Attributes_1__Name\">");
        }
        
        [Test]
        public void Should_overwrite_css_when_set_class_has_value()
        {
            // arrange
            var model = new Command { Name = "John" };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Name);
            tag.SetClass = "the-final-form-input";

            // act
            var html = ProcessTag(tag, "miru-input", new { @class = "form-control" });
            
            // assert
            html.HtmlShouldBe("<input type=\"text\" value=\"John\" name=\"Name\" id=\"Name\" class=\"the-final-form-input\">");
        }

        public class Command
        {
            [Radio]
            public Relationships Relationship { get; set; }
            
            public string Name { get; set; }

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
        
        public enum Relationships
        {
            Single,
            Married,
            Divorced
        }
    }

}