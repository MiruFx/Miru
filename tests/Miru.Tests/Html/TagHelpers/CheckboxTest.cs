using System.Collections.Generic;
using Miru.Html;
using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class CheckboxTest  : TagHelperTest
{
    [Test]
    public async Task Render_checkbox_for_boolean_false()
    {
        // arrange
        var model = new Command { Remember = false };
        var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-input");
            
        // assert
        html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\">");
    }
        
    [Test]
    public async Task  Render_checkbox_for_boolean_true()
    {
        // arrange
        var model = new Command { Remember = true };
        var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-input");
            
        // assert
        html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\" checked=\"checked\">");
    }
        
    [Test]
    public async Task  Can_override_default_attributes()
    {
        // arrange
        var model = new Command { Remember = false };
        var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.Remember);
            
        // act
        var html = await ProcessTagAsync(tag, "miru-input", new { @checked = "checked" });
            
        // assert
        html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\" checked=\"checked\">");
    }
 
    [Test]
    public async Task  When_input_is_checkbox_and_property_list_of_enum_should_set_check_existent_values()
    {
        // arrange
        var model = new Command { NewsletterOptions = new[] { Newsletters.Offers, Newsletters.Partners }};
        var tag = CreateTagWithFor(new InputTagHelper(), model, m => m.NewsletterOptions);

        // act
        var htmlPartners = await ProcessTagAsync(tag, "miru-input", new { value = Newsletters.Partners });
        var htmlNews = await ProcessTagAsync(tag, "miru-input", new { value = Newsletters.News });
            
        // assert
        htmlPartners.HtmlShouldBe(
            $"<input type=\"checkbox\" value=\"Partners\" name=\"NewsletterOptions\" id=\"NewsletterOptions\" checked=\"checked\">");
            
        htmlNews.HtmlShouldBe(
            $"<input type=\"checkbox\" value=\"News\" name=\"NewsletterOptions\" id=\"NewsletterOptions\">");
    }
        
    public class Command
    {
        [Checkbox]
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