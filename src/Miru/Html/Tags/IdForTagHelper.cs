using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "id-for")]
public class IdForTagHelper : MiruTagHelper
{
    [HtmlAttributeName("id-for")]
    public ModelExpression IdFor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.SetAttribute("id", ElementNaming.Id(IdFor));
    }
}