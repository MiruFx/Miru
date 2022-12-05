using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "miru-if")]
public class IfTagHelper : TagHelper
{
    [HtmlAttributeName("miru-if")]
    public bool When { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (When == false)
        {
            output.SuppressOutput();
        }
    }
}