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
        
        public class Command
        {
            public DateTime? CreatedAt { get; set; }
        }
    }
}