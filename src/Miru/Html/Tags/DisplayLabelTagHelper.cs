using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-display-label", Attributes = "x")]
[HtmlTargetElement("miru-display-label", Attributes = "for")]
[HtmlTargetElement("miru-display-label", Attributes = "model")]
public class DisplayLabelTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Span;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.DisplayLabelFor(ElementRequest.Create(this), output);
    }
}