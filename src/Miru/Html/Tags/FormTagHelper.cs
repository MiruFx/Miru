using Baseline;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-form")]
    public class FormTagHelper : MiruForTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = For != null ? For.Model : ViewContext.ViewData.Model;
            
            var form = HtmlGenerator.FormFor(model);

            var url = RequestServices.GetService<UrlLookup>().For(model);
            
            if (url.IsNotEmpty())
                form.Attr("action", RequestServices.GetService<UrlLookup>().For(model));
            
            form.MergeAttributes(output.Attributes);
            
            output.TagName = null;
            output.PreElement.SetHtmlContent(form.ToHtmlString());
        }
    }
}