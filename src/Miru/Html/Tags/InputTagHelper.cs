using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-input", Attributes = "x")]
[HtmlTargetElement("miru-input", Attributes = "for")]
[HtmlTargetElement("miru-input", Attributes = "model")]
public class InputTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Input;
        output.TagMode = TagMode.SelfClosing;

        TagModifier.InputFor(ElementRequest.Create(this), output);
    }
}