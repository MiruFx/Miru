using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-table", Attributes = "for")]
    public class TableTagHelper : MiruForTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            EnsureForIsValid();
            
            var model = For.Model as IEnumerable;
            
            if (model == null)
                throw new ArgumentException("Attribute 'for' has to be a IEnumerable");
            
            if (model.GetEnumerator().MoveNext())
            {
                output.TagName = "table";
                output.Attributes.Add("id", ElementNaming.Id(For.Metadata.ContainerType));
            }
            else
            {
                output.SuppressOutput();
            }
        } 
    }
}