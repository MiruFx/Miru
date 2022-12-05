using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-display", Attributes = "x")]
[HtmlTargetElement("miru-display", Attributes = "for")]
[HtmlTargetElement("miru-display", Attributes = "model")]
public class DisplayTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // configure tag
        output.TagName = HtmlAttr.Span;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.ModifyDisplayFor(ElementRequest.Create(this), output);
    }
}