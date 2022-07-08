using Miru.Html.Tags;

namespace Miru.Tests.Html.TagHelpers;

public class FormTagHelperTest : MiruTagTesting
{
    [Test]
    public async Task If_method_is_get_should_not_add_summary_hidden_and_anti_forgery_inputs()
    {
        // arrange
        var tag = CreateTagWithModel(new FormTagHelper(), new PostNew.Command());
        
        // act
        var output = await ProcessTagAsync(tag, "miru-form", new { method = "get" });
            
        // assert
        output.HtmlShouldBe(
            "<form method=\"get\" id=\"post-new-form\" data-form-summary=\"post-new-summary\" data-controller=\"form\" action=\"/PostNew\">");
        output.PostHtmlShouldBe("</form>");
    }
        
    [Test]
    public async Task If_no_attributes_method_should_be_post()
    {
        // arrange
        var tag = CreateTagWithModel(new FormTagHelper(), new PostNew.Command());
        
        // act
        var output = await ProcessTagAsync(tag, "miru-form");
            
        // assert
        output.HtmlShouldBe(
            @"<form method=""post"" id=""post-new-form"" data-form-summary=""post-new-summary"" data-controller=""form"" action=""/PostNew""><input type=""hidden"" name=""FormFieldName"" value=""RequestToken"">");
        output.PostHtmlShouldBe("</form>");
    }
    
    public class PostNew
    {
        public class Command
        {
            public string Title { get; set; }
        }
    }
    
    public class PostArchive
    {
        public class Query
        {
            public string ToDate { get; set; }
        }
    }
}
