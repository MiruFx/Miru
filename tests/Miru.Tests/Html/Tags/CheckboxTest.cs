using Miru.Html.Tags;
using Miru.Tests.Html.TagHelpers;
using NUnit.Framework;

namespace Miru.Tests.Html.Tags
{
    public class CheckboxTest  : TagHelperTest
    {
        [Test]
        public void Render_checkbox_for_boolean_false()
        {
            // arrange
            var model = new Login { Remember = false };
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
            var model = new Login { Remember = true };
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
            var model = new Login { Remember = false };
            var tag = CreateTag(new InputTagHelper(), model, m => m.Remember);
            
            // act
            var html = ProcessTag(tag, "miru-input", new { @checked = "checked" });
            
            // assert
            html.HtmlShouldBe("<input type=\"checkbox\" value=\"true\" name=\"Remember\" id=\"Remember\" checked=\"checked\">");
        }

        public class Login
        {
            public bool Remember { get; set; }
        }
    }
}