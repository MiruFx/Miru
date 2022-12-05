using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public class TagModifier
{
    private readonly HtmlConventions _htmlConventions;

    public TagModifier(HtmlConventions htmlConventions)
    {
        _htmlConventions = htmlConventions;
    }

    public void ModifyDisplayFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Displays, output);
    
    public void ModifyFormFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Forms, output);
    
    public void ModifyTableFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Tables, output);
    
    public void InputFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Inputs, output);
    
    public void SelectFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Selects, output);
    
    public void TableHeaderFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.TableHeaders, output);
    
    public void TableCellFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.TableCells, output);
    
    public void LabelFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.Labels, output);
    
    public void DisplayLabelFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.DisplayLabels, output);
    
    public void FormSummaryFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.FormSummaries, output);
    
    public void ValidationMessageFor(ElementRequest elementRequest, TagHelperOutput output) =>
        ModifyTag(elementRequest, _htmlConventions.ValidationMessages, output);
    
    private void ModifyTag(
        ElementRequest elementRequest, 
        ITagConventions tagConventions,
        TagHelperOutput output)
    {        
        if (tagConventions is IConventionAccessor accessor)
            foreach (var convention in accessor.Modifiers)
                if (convention.Condition(output, elementRequest))
                    convention.Modification(output, elementRequest);
        
        // TODO: MIRU 2.0 make this as GlobalModifier se it can be configurable
        
        // handle css class attributes
        if (output.Attributes.TryGetAttribute("set-class", out var setClassAttr))
        {
            output.Attributes.SetAttribute(HtmlAttr.Class, setClassAttr.Value.ToString());
            output.Attributes.Remove(setClassAttr);
        }
    }
}