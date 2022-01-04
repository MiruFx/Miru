using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "miru-if-has")]
public class IfHasTagHelper : TagHelper
{
    [HtmlAttributeName("miru-if-has")]
    public object Instance { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Instance == null)
        {
            output.SuppressOutput();
        }
    }
}