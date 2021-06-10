using System.Collections.Generic;
using Miru.Html;
using Miru.Html.Tags;
using Miru.Tests.Html.TagHelpers;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.Tags
{
    public class CheckboxTest  : TagHelperTest
    {
        [Test]
        public void Render_checkbox_for_boolean_false()
        {
            // arrange
            var model = new Command { Remember = false };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Remember);
            
            // act
            var html = ProcessTag(tag, "miru-input");
            
            // assert
            html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\">");
        }
        
        [Test]
        public void Render_checkbox_for_boolean_true()
        {
            // arrange
            var model = new Command { Remember = true };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Remember);
            
            // act
            var html = ProcessTag(tag, "miru-input");
            
            // assert
            html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\" checked=\"checked\">");
        }
        
        [Test]
        public void Can_override_default_attributes()
        {
            // arrange
            var model = new Command { Remember = false };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Remember);
            
            // act
            var html = ProcessTag(tag, "miru-input", new { @checked = "checked" });
            
            // assert
            html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\" checked=\"checked\">");
        }
 
        [Test]
        public void When_input_is_checkbox_and_property_list_of_enum_should_set_check_existent_values()
        {
            // arrange
            var model = new Command { NewsletterOptions = new[] { Newsletters.Offers, Newsletters.Partners }};
            var tag = CreateTag(new InputTagHelper(), model, m => m.NewsletterOptions);

            // act
            var htmlPartners = ProcessTag(tag, "miru-input", new { value = Newsletters.Partners });
            var htmlNews = ProcessTag(tag, "miru-input", new { value = Newsletters.News });
            
            // assert
            htmlPartners.HtmlShouldBe(
                $"<input type=\"checkbox\" value=\"Partners\" name=\"NewsletterOptions\" id=\"NewsletterOptions\" checked=\"checked\">");
            
            htmlNews.HtmlShouldBe(
                $"<input type=\"checkbox\" value=\"News\" name=\"NewsletterOptions\" id=\"NewsletterOptions\">");
        }
        
        public class Command
        {
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
}