using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.Tags
{
    public abstract class MiruForTagHelper : MiruTagHelperBase
    {
        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        protected void EnsureForIsValid()
        {
            if (For == null)
                throw new InvalidOperationException(
                    "Missing or invalid 'for' attribute value. Specify a valid model expression for the 'for' attribute value.");
        }
    }
}