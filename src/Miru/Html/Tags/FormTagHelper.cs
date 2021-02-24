using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
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

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // FIXME: send upper to base
            var model = Model ?? (For != null ? For.Model : ViewContext.ViewData.Model);
            
            var form = HtmlGenerator.FormFor(model);

            if (output.Attributes.ContainsName("action") == false)
            {
                var url = RequestServices.GetRequiredService<UrlLookup>().For(model);

                if (url.IsNotEmpty())
                    output.Attributes.Add("action", url);
            }
            
            if (output.Attributes["method"] == null || !output.Attributes["method"].Value.ToString().CaseCmp("get"))
            {
                var antiforgeryAccessor = RequestServices.GetService<IAntiforgeryAccessor>();

                if (antiforgeryAccessor.HasToken)
                {
                    var input = new HtmlTag("input")
                        .Attr("type", "hidden")
                        .Name(antiforgeryAccessor.FormFieldName)
                        .Value(antiforgeryAccessor.RequestToken);
                
                    form.Append(input);
                }
                
                form.Add("input", tag =>
                {
                    var summaryId = ElementNaming.FormSummaryId(model);

                    tag.Attr("type", "hidden")
                        .Attr("name", "__Summary")
                        .Attr("value", summaryId);
                });
            }
            
            form.MergeAttributes(output.Attributes);
            
            output.TagName = null;
            output.PreElement.AppendHtml(form);
            output.PostElement.AppendHtml("</form>");
        }
    }
}