using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "miru-if-any")]
public class IfAnyTagHelper : TagHelper
{
    [HtmlAttributeName("miru-if-any")]
    public object Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model is IEnumerator enumerator && enumerator.MoveNext() == false)
        {
            output.SuppressOutput();
        }
        else if (Model is string text && string.IsNullOrEmpty(text))
        {
            output.SuppressOutput();
        }
        else if (Model is null)
        {
            output.SuppressOutput();
        }
    }
}