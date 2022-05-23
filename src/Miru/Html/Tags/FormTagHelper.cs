using System;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-form")]
public class FormTagHelper : MiruForTagHelper
{
    protected override string Category => nameof(HtmlConfiguration.Forms);

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (htmlTag.HasAttr("action") == false)
        {
            var url =  UrlLookup.For(builder.Model);

            if (url.IsNotEmpty())
            {
                htmlTag.Attr("action", url);
            }
        }
            
        if (htmlTag.HasAttr("method") == false || htmlTag.Attr("method").CaseCmp("get") == false) 
        {
            var antiForgeryAccessor = RequestServices.GetRequiredService<IAntiforgeryAccessor>();

            if (antiForgeryAccessor.HasToken)
            {
                var input = new HtmlTag("input")
                    .Attr("type", "hidden")
                    .Name(antiForgeryAccessor.FormFieldName)
                    .Value(antiForgeryAccessor.RequestToken);
                
                htmlTag.Append(input);
            }
        }
    }

    public override void AfterSetHtmlContent(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        builder.Output.PostElement.AppendHtml("</form>");
    }
}