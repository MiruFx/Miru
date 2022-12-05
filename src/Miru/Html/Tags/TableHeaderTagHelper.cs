using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-th")]
[HtmlTargetElement("miru-th", Attributes = "for")]
[HtmlTargetElement("miru-th", Attributes = "model")]
public class TableHeaderTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Th;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.TableHeaderFor(ElementRequest.Create(this), output);
    }
}