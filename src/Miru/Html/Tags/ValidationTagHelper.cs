using System;
using System.Collections;
using System.Collections.Generic;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-validation", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ValidationTagHelper : MiruHtmlTagHelper
    {
        protected override string Category => ElementConstants.ValidationMessage;

        protected override void BeforeRender(TagHelperOutput tagHelperOutput, HtmlTag htmlTag)
        {
        }
    }
}