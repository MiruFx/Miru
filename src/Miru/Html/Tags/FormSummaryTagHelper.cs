using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-summary")]
public class FormSummaryTagHelper : MiruTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Div;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.FormSummaryFor(ElementRequest.Create(this), output);
    }
}