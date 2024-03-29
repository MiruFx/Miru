using Baseline;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "src-for")]
public class SrcForTagHelper : MiruTagHelper
{
    [HtmlAttributeName("src-for")]
    public object For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For != null)
        {
            var url = RequestServices.GetRequiredService<UrlLookup>();

            if (For is ModelExpression modelExpression)
            {
                output.Attributes.SetAttribute("src", url.For(modelExpression.Model));
            }
            else
            {
                output.Attributes.SetAttribute("src", url.For(For));
            }
        }
    }
}