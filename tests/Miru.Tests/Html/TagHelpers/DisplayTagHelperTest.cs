using System;
using System.Threading.Tasks;
using Miru.Html.Tags;
using NUnit.Framework;

namespace Miru.Tests.Html.TagHelpers
{
    public class DisplayTagHelperTest : TagHelperTest
    {
        [Test]
        public async Task When_property_is_null_should_render_empty_span()
        {
            // arrange
            var model = new Command { CreatedAt = null };
            var tag = CreateTag(new DisplayTagHelper(), model, m => m.CreatedAt);
            
            // act
            var html = await ProcessTagAsync(tag, "miru-display");
            
            // assert
            html.HtmlShouldBe("<span id=\"CreatedAt\"></span>");
        }
        
        [Test]
        public void Should_render_class()
        {
            // arrange
            var model = new Command { Total = 10 };
            var tag = CreateTag(new DisplayTagHelper(), model, m => m.Total);
            
            // act
            var html = ProcessTag(tag, "miru-display", new { @class = "small" });
            
            // assert
            html.HtmlShouldBe("<span id=\"Total\" class=\"small\">10</span>");
        }
        
        public class Command
        {
            public DateTime? CreatedAt { get; set; }
            public int Total { get; set; }
        }
    }
}