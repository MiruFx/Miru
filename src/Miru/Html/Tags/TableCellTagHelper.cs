using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-td")]
[HtmlTargetElement("miru-td", Attributes = "for")]
[HtmlTargetElement("miru-td", Attributes = "model")]
public class TableCellTagHelper : MiruForTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Td;
        output.TagMode = TagMode.StartTagAndEndTag;

        TagModifier.TableCellFor(ElementRequest.Create(this), output);
    }
}