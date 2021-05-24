using System;
using System.Collections;
using System.Collections.Generic;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-input", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class InputTagHelper : MiruHtmlTagHelper
    {
        protected override string Category { get; } = ElementConstants.Editor;

        protected override void BeforeRender(TagHelperOutput tagHelperOutput, HtmlTag htmlTag)
        {
            if (htmlTag.Attr("type").Equals("radio"))
            {
                if (tagHelperOutput.Attributes.TryGetAttribute("value", out TagHelperAttribute attribute))
                {
                    if (attribute.Value.ToString()!.Equals(For.Model.ToString()))
                        htmlTag.Checked();
                }
            }
            
            if (htmlTag.Attr("type").Equals("checkbox"))
            {
                if (tagHelperOutput.Attributes.TryGetAttribute("value", out TagHelperAttribute attribute))
                {
                    if (For.Model.GetType().IsArray)
                    {
                        var list = For.Model as IEnumerable;
                        
                        foreach (var item in list)
                        {
                            if (item.ToString()!.Equals(attribute.Value.ToString()))
                                htmlTag.Checked();
                        }
                    }
                    if (attribute.Value.ToString()!.Equals(For.Model.ToString()))
                        htmlTag.Checked();
                }
                else if (For.Model is true)
                {
                    htmlTag.Checked();
                }
            }
        }
    }
}