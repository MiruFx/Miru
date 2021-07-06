using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-display", Attributes = ForAttributeName)]
    public class DisplayTagHelper : MiruHtmlTagHelper
    {
        protected override string Category => ElementConstants.Display;
    }
}