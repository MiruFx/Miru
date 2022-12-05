using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs;

public class FormTagHelperTest : MiruTagTesting
{
    [Test]
    public void Should_render_default_form()
    {
        // arrange
        var tag = TagWithModel<FormTagHelper>(new PostNew.Command());
        
        // act
        var output = ProcessTag(tag, "miru-form");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<form id=""post-new-form"" action=""/PostNew"" method=""post""><input name=""FormFieldName"" type=""hidden"" value=""RequestToken"" /></form>");
    }
    
    [Test]
    public void Should_render_form_with_method_get()
    {
        // arrange
        var tag = TagWithModel<FormTagHelper>(new PostNew.Command());
        
        // act
        var output = ProcessTag(tag, "miru-form", new { method = "get" });
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, "<form method=\"get\" id=\"post-new-form\" action=\"/PostNew\"></form>");
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