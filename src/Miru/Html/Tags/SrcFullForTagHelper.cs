using Baseline;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "full-src")]
public class SrcFullForTagHelper : MiruTagHelper
{
    [HtmlAttributeName("full-src")]
    public string FullSrc { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (FullSrc.IsNotEmpty())
        {
            var urlOptions = RequestServices.GetRequiredService<IOptions<UrlOptions>>();

            var src = $"{urlOptions.Value.Base}{FullSrc}";
        
            output.Attributes.SetAttribute("src", src);
        }
    }
}