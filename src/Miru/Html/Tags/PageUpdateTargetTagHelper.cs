using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("template", Attributes = "data-page-update-target")]
    public class PageUpdateTargetTagHelper : MiruTagHelperBase
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.HttpContext.Request.Headers.TryGetValue("X-Miru-Target", out var targetId))
                output.Attributes.SetAttribute("data-page-update", "update#" + targetId);
        }
    }
}