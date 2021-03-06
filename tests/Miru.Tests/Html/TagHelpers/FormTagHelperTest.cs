using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class FormTagHelperTest : TagHelperTest
    {
        [Test]
        public void If_method_is_get_should_not_add_summary_hidden_and_anti_forgery_inputs()
        {
            // arrange
            var tag = new FormTagHelper
            {
                For = MakeExpression(new PostNew.Command()),
                RequestServices = ServiceProvider
            };
        
            // act
            var output = ProcessTag(tag, "miru-form", new TagHelperAttributeList
            {
                { "method", "get" },
            });
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe(
                "<form method=\"get\" id=\"post-new-form\" data-form-summary=\"post-new-summary\" data-controller=\"form\" action=\"/PostNew\">");
            output.PostElement.GetContent().ShouldBe("</form>");
        }
        
        [Test]
        public void If_no_attributes_method_should_be_post()
        {
            // arrange
            var tag = new FormTagHelper
            {
                For = MakeExpression(new PostNew.Command()),
                RequestServices = ServiceProvider
            };
        
            // act
            var output = ProcessTag(tag, "miru-form");
            
            // assert
            output.TagName.ShouldBeNull();
            output.PreElement.GetContent().ShouldBe(
@"<form method=""post"" id=""post-new-form"" data-form-summary=""post-new-summary"" data-controller=""form"" action=""/PostNew""><input type=""hidden"" name=""FormFieldName"" value=""RequestToken"">");
            output.PostElement.GetContent().ShouldBe("</form>");
        }
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