using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-submit", TagStructure = TagStructure.WithoutEndTag)]
    public class SubmitTagHelper : NoPropertyTagHelper
    {
        protected override string Category { get; } = nameof(HtmlConvention.Submits);
    }
}