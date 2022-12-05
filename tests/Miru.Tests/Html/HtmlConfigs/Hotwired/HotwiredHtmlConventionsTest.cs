using Miru.Html.HtmlConfigs;
using Miru.Html.Tags;
using Miru.Testing.Html;

namespace Miru.Tests.Html.HtmlConfigs.Hotwired;

public class HotwiredHtmlConventionsTest : MiruTagTesting
{
    protected override void HtmlConfig(HtmlConventions htmlConfig)
    {
        // class under test
        htmlConfig.AddMiruHotwired();
    }

    [Test]
    public void Should_render_form()
    {
        // arrange
        var tag = TagWithModel<FormTagHelper>(new PostNew.Command());
        
        // act
        var output = ProcessTag(tag, "miru-form");
            
        // assert
        Miru.Testing.Html.Extensions.HtmlShouldBe(output, @"<form id=""post-new-form"" data-controller=""form"" data-form-summary=""post-new-summary"" action=""/PostNew"" method=""post""><input name=""FormFieldName"" type=""hidden"" value=""RequestToken"" /></form>");
    }
    
    public class PostNew
    {
        public class Command
        {
            public string Title { get; set; }
        }
    }
}