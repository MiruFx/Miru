using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("a", Attributes = "for")]
public class LinkForTagHelper : MiruTagHelper
{
    [HtmlAttributeName("for")]
    public object For { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For != null)
        {
            var url = RequestServices.GetRequiredService<UrlLookup>();
                
            if (For is ModelExpression modelExpression)
                output.Attributes.SetAttribute("href", url.For(modelExpression.Model));
            else
                output.Attributes.SetAttribute("href", url.For(For));
        }
    }
}