using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "miru-if-any")]
public class IfAnyTagHelper : TagHelper
{
    [HtmlAttributeName("miru-if-any")]
    public IEnumerable Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model.GetEnumerator().MoveNext() == false)
        {
            output.SuppressOutput();
        }
    }
}