using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Domain;
using Miru.Mvc;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-select", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SelectTagHelper : MiruHtmlTagHelper
    {
        [HtmlAttributeName("lookup")]
        public ModelExpression Lookup { get; set; }

        protected override string Category { get; } = nameof(HtmlConfiguration.Selects);

        protected override void BeforeRender(TagHelperOutput output, HtmlTag htmlTag)
        {
            var selectTag = (SelectTag) htmlTag;
        
            if (Lookup != null)
            {
                if (Lookup.Model is IEnumerable<ILookupable> lookupables)
                {
                    lookupables.Each(item => selectTag.Option(item.Display, item.Value));
                }
                
                if (Lookup.Model is Lookups lookups)
                {
                    lookups.ForEach(item => selectTag.Option(item.Description, item.Id));
                }
            }
            else
            {
                if (For.Model.GetType().IsEnum)
                {
                    foreach (var enumValue in Enum.GetValues(For.Model.GetType()))
                    {
                        var displayAttribute = For.Model.GetType().GetMember(enumValue.ToString()!)
                            .First()
                            .GetCustomAttribute<DisplayAttribute>();
        
                        var description = displayAttribute != null ? 
                            displayAttribute.Name : 
                            Enum.GetName(For.Model.GetType(), enumValue);
                        
                        // selectTag.Option(description, Convert.ToInt32(enumValue));
                        selectTag.Option(description, enumValue);
                    }
                }
            }
        
            if (For.Model != null) selectTag.SelectByValue(For.Model);
        
            if (output.Attributes.ContainsName("empty-option"))
            {
                var emptyOption = output.Attributes["empty-option"].Value.ToString();
                
                selectTag.EmptyOption(emptyOption);
            }
        
            selectTag.MergeAttributes(output.Attributes);
        }

        private void ThrowIfLookupableIsInvalid()
        {
            if (For == null)
                throw new InvalidOperationException(
                    "Missing or invalid 'lookup' attribute value. It has to implement IEnumerable<ILookupable>.");
        }
    }
}