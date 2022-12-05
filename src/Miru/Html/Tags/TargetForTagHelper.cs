using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "target-for")]
public class TargetForTagHelper : MiruForTagHelper
{
    [HtmlAttributeName("target-for")]
    public ModelExpression TargetFor { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.SetAttribute("target", ElementNaming.Id(TargetFor));
    }
}