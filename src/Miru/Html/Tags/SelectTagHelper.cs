using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Domain;

namespace Miru.Html.Tags
{
    [HtmlTargetElement("miru-select", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class SelectTagHelper : MiruForTagHelper
    {
        [HtmlAttributeName("lookup")]
        public ModelExpression Lookup { get; set; }
        
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            EnsureForIsValid();
            ThrowIfLookupableIsInvalid();

            var selectTag = (SelectTag) HtmlGenerator.TagFor(For, nameof(HtmlConfiguration.Selects));

            if (Lookup != null)
            {
                if (Lookup.Model is IEnumerable<ILookupable> lookupables)
                {
                    lookupables.Each(item => selectTag.Option(item.Display, item.Value));
                }
            }

            var currentValue = For.Model;
            
            selectTag.SelectByValue(currentValue);

            if (output.Attributes.ContainsName("empty-option"))
            {
                var emptyOption = output.Attributes["empty-option"].Value.ToString();
                
                selectTag.EmptyOption(emptyOption);
            }

            selectTag.MergeAttributes(output.Attributes);
            
            output.TagName = null;
            output.PreElement.SetHtmlContent(selectTag.ToHtmlString());

            return Task.CompletedTask;
        }

        private void ThrowIfLookupableIsInvalid()
        {
            if (For == null)
                throw new InvalidOperationException(
                    "Missing or invalid 'lookup' attribute value. It has to implement IEnumerable<ILookupable>.");
        }
    }
}