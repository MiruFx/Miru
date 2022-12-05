using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-label", Attributes = "x")]
[HtmlTargetElement("miru-label", Attributes = "for")]
[HtmlTargetElement("miru-label", Attributes = "model")]
public class LabelTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Label;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.LabelFor(ElementRequest.Create(this), output);
    }
}