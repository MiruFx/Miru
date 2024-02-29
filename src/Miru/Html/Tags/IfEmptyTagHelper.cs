using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "miru-if-empty")]
public class IfEmptyTagHelper : TagHelper
{
    [HtmlAttributeName("miru-if-empty")]
    public object Instance { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Instance is IEnumerable enumerable && enumerable.GetEnumerator().MoveNext())
            output.SuppressOutput();
        
        // else if (Instance == null) 
        //     output.SuppressOutput();
        
        else if (Instance != null && Instance.Equals(Instance.GetType().GetDefault()) == false) 
            output.SuppressOutput();
    }
}