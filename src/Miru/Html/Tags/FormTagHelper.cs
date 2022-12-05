using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.HtmlConfigs;
using Miru.Urls;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-form")]
[HtmlTargetElement("miru-form", Attributes = "for")]
[HtmlTargetElement("miru-form", Attributes = "model")]
public class FormTagHelper : MiruTagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlAttr.Form;
        output.TagMode = TagMode.StartTagAndEndTag;

        var req = ElementRequest.Create(this);
        
        TagModifier.ModifyFormFor(req, output);
        
        // TODO: move to conventions
        if (req.Value is not null)
        {
            if (output.Attributes.ContainsName(HtmlAttr.Action) == false)
            {
                var url = RequestServices.Get<UrlLookup>().For(req.Value);
                if (url is not null)
                    output.Attributes.SetAttribute(HtmlAttr.Action, url);
            }
        }

        if (output.Attributes.ContainsName("method") == false)
            output.Attributes.SetAttribute("method", "post");
        
        if (output.Attributes.GetValue("method") == "post")
        {
            var antiforgeryAccessor = RequestServices.Get<IAntiforgeryAccessor>();
            var antiforgeryTag = new TagBuilder("input");
            antiforgeryTag.Attributes.Add("type", "hidden");
            antiforgeryTag.Attributes.Add("name", antiforgeryAccessor.FormFieldName);
            antiforgeryTag.Attributes.Add("value", antiforgeryAccessor.RequestToken);
            antiforgeryTag.TagRenderMode = TagRenderMode.SelfClosing;
            output.PostContent.AppendHtml(antiforgeryTag);
        }
    }
}