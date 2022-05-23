using System.Collections;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-input", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
public class InputTagHelper : MiruForTagHelper
{
    protected override string Category => ElementConstants.Editor;

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (htmlTag.Attr("type").Equals("radio"))
        {
            if (htmlTag.HasAttr("value"))
            {
                if (For.Model != null &&
                    htmlTag.Value().Equals(For.Model.ToString()))
                {
                    htmlTag.Checked();
                }
            }
        }
            
        if (htmlTag.Attr("type").Equals("checkbox"))
        {
            if (htmlTag.HasAttr("value"))
            {
                if (For.Model.GetType().IsArray)
                {
                    var list = For.Model as IEnumerable;
                        
                    foreach (var item in list)
                    {
                        if (item.ToString()!.Equals(htmlTag.Value()))
                        {
                            htmlTag.Checked();
                        }
                    }
                }

                if (htmlTag.Value().Equals(For.Model.ToString()!.ToLower()))
                {
                    htmlTag.Checked();
                }
            }
            else
            {
                htmlTag.Value("true");
            }
                
            if (For.Model is true)
            {
                htmlTag.Checked();
            }
        }
    }
}