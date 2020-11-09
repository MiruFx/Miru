using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-display", Attributes = ForAttributeName)]
    [HtmlTargetElement("md", Attributes = ForAttributeName)]
    public class DisplayTagHelper : HtmlTagTagHelper
    {
        protected override string Category => ElementConstants.Display;
    }
}