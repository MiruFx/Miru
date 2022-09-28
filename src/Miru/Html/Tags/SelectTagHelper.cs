using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AV.Enumeration;
using Baseline;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Domain;
using Miru.Mvc;

namespace Miru.Html.Tags;

[HtmlTargetElement("miru-select", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
public class SelectTagHelper : MiruForTagHelper
{
    [HtmlAttributeName("lookup")]
    public ModelExpression Lookup { get; set; }

    protected override string Category => nameof(HtmlConfiguration.Selects);

    public override void AfterHtmlTagGeneration(MiruTagBuilder builder, HtmlTag htmlTag)
    {
        if (htmlTag is SelectTag selectTag)
        {
            if (Lookup != null)
            {
                if (Lookup.Model is IEnumerable<ILookupable> lookupables)
                {
                    lookupables.Each(item => selectTag.Option(item.Display, item.Value));
                }
                
                if (Lookup.Model is SelectLookups lookups)
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

            if (For.Model != null)
            {
                // if (For.Model.GetType().ImplementsGenericOf(typeof(Enumeration<>)))
                if (For.Model is Enumeration)
                {
                    selectTag.SelectByValue(For.Model.GetPropertyValue("Value"));
                }
                else if (For.Model is Enum @enum)
                {
                    selectTag.SelectByValue(@enum.ToInt());
                }
                else
                {
                    selectTag.SelectByValue(For.Model);
                }
            }
        
            if (selectTag.HasAttr("empty-option"))
            {
                var emptyOption = selectTag.Attr("empty-option");
                
                selectTag.EmptyOption(emptyOption);
            }
        }
    }
}