using System;
using Baseline;
using HtmlTags;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-label", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class LabelTagHelper : HtmlTagTagHelper
    {
        protected override string Category { get; } = ElementConstants.Label;

        [HtmlAttributeName("set-class")]
        protected string SetClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            //
            // if (SetClass.IsNotEmpty())
            //     output.Attributes.SetAttribute("class", SetClass);
        }
    }
    
    [HtmlTargetElement("ml", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MlTagHelper : LabelTagHelper
    {
    }
}