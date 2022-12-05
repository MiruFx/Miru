using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-table", Attributes = "add-class")]
public class AddClassTagHelper : MiruForTagHelper
{
    [HtmlAttributeName("add-class")]
    public string AddClass { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.Append(HtmlAttr.Class, AddClass);
    }
}