using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-table", Attributes = "for")]
    public class TableTagHelper : MiruHtmlTagHelper
    {
        protected override string Category { get; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = For.Model as IEnumerable;
            
            if (model == null)
                throw new ArgumentException("Attribute 'for' has to be a IEnumerable");
            
            if (model.GetEnumerator().MoveNext())
            {
                var modelType = For.Metadata.ContainerType ?? For.ModelExplorer.Container.ModelType;
                
                output.TagName = "table";
                output.Attributes.Add("id", ElementNaming.Id(modelType));
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}