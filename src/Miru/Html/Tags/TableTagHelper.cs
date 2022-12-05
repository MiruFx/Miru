using System.Collections;
using Baseline;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-table")]
[HtmlTargetElement("miru-table", Attributes = "for")]
[HtmlTargetElement("miru-table", Attributes = "model")]
public class TableTagHelper : MiruTagHelper
{
    [HtmlAttributeName("add-class")]
    public string AddClass { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var req = ElementRequest.Create(this);
        
        if (req.Value is IEnumerable list && list.GetEnumerator().MoveNext() == false)
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = HtmlAttr.Table;
            output.TagMode = TagMode.StartTagAndEndTag;

            TagModifier.ModifyTableFor(req, output);
            
            if (AddClass.IsNotEmpty())
                output.Attributes.Append(HtmlAttr.Class, AddClass);
        }
    }
}