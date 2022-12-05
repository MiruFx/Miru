using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-validation", Attributes = "x")]
[HtmlTargetElement("miru-validation", Attributes = "for")]
[HtmlTargetElement("miru-validation", Attributes = "model")]
public class ValidationTagHelper : MiruTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Div;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.ValidationMessageFor(ElementRequest.Create(this), output);
    }
}