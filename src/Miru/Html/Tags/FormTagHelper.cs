using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-form")]
    public class FormTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("model")]
        public object Model
        {
            get;
            set;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var model = Model ?? (For != null ? For.Model : ViewContext.ViewData.Model);
            
            var form = HtmlGenerator.FormFor(model);

            if (output.Attributes.ContainsName("action") == false)
            {
                var url = RequestServices.GetService<UrlLookup>().For(model);

                if (url.IsNotEmpty())
                    output.Attributes.Add("action", RequestServices.GetService<UrlLookup>().For(model));
            }

            form.MergeAttributes(output.Attributes);
            
            output.TagName = null;
            output.PreElement.AppendHtml(form);
            output.PostElement.AppendHtml("</form>");
        }
    }
    
    [HtmlTargetElement("form", Attributes = "for")]
    public class NewFormTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("model")]
        public object Model
        {
            get;
            set;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var model = Model ?? (For != null ? For.Model : ViewContext.ViewData.Model);
            
            var form = HtmlGenerator.FormFor(model);
            
            if (output.Attributes.ContainsName("action") == false)
            {
                var url = RequestServices.GetService<UrlLookup>().For(model);

                if (url.IsNotEmpty())
                    output.Attributes.Add("action", RequestServices.GetService<UrlLookup>().For(model));
            }

            // output.MergeAttributes(output.Attributes);

            var childrenContent = form.Children.Select(x => x.ToHtmlString()).Join(string.Empty);
            
            output.TagName = "form";
            output.Content.SetHtmlContent(childrenContent);
            // output.PostElement.SetHtmlContent();   
        }
    }
}