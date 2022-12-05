using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("*", Attributes = "full-src")]
public class FullSrcTagHelper : MiruForTagHelper
{
    [HtmlAttributeName("full-src")]
    public string FullSrc { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var urlOptions = RequestServices.GetRequiredService<IOptions<UrlOptions>>();

        var src = $"{urlOptions.Value.Base}{FullSrc}";
        
        output.Attributes.SetAttribute("src", src);
    }
}