using HtmlTags;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-display-label", Attributes = "for")]
    public class DisplayLabelTagHelper : HtmlTagTagHelper
    {
        protected override string Category => nameof(HtmlConfiguration.DisplayLabels);
    }
}