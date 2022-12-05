using Miru.Html.Tags;
using Miru.Tests.Html.HtmlConfigs.Helpers;

namespace Miru.Tests.Html.HtmlConfigs;

public class FormSummaryTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_form_summary()
    {
        // arrange
        var model = new PostNew.Command();
        var tag = TagWithModel<FormSummaryTagHelper>(model);
        
        // act
        var output = ProcessTag(tag, "miru-summary2");
            
        // assert
        Helpers.Extensions.HtmlShouldBe(output, @"<div id=""post-new-summary""></div>");
    }
    
    public class PostNew
    {
        public class Command
        {
            public string Title { get; set; }
        }
    }
}