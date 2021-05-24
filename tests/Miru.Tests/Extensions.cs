using Microsoft.AspNetCore.Razor.TagHelpers;
using Shouldly;

namespace Miru.Tests
{
    [ShouldlyMethods]
    public static class Extensions
    {
        public static void HtmlShouldBe(this TagHelperOutput tagHelperOutput, string expectedHtml)
        {
            tagHelperOutput.PreElement.GetContent().ShouldBe(expectedHtml);
        }
    }
}