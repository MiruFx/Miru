using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.Tags;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html.TagHelpers
{
    public class FormTagHelperTest : TagHelperTest
    {
        private Command _request;

        [SetUp]
        public void Setup()
        {
            _request = new Command();
        }
        
        [Test]
        public void If_method_is_get_should_not_add_summary_hidden_and_anti_forgery_inputs()
        {
            // arrange
            var tag = new FormTagHelper
            {
                For = MakeExpression(_request),
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
                "<form method=\"get\" id=\"form-tag-helper-test-form\" action=\"/Object\">");
            output.PostElement.GetContent().ShouldBe("</form>");
        }
     
        public class Command
        {
            public string Name { get; set; }
        }
    }
}