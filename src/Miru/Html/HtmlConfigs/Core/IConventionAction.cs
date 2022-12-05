using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html.Tags;

namespace Miru.Html.HtmlConfigs.Core;

public interface IConventionAction
{
    IConventionAction Modify(Action<TagHelperOutput, ElementRequest> modifier);
}

public static class ConventionActionExtensions
{
    public static IConventionAction Modify(this IConventionAction convention, Action<TagHelperOutput> modifier) =>
        convention.Modify((tag, _) => modifier(tag));
    
    public static IConventionAction Modify<TPropertyType>(
        this IConventionAction convention, 
        Action<TagHelperOutput, ElementRequest, TPropertyType> modifier) =>
            convention.Modify((tag, req) =>
            {
                if (req.Value is TPropertyType value)
                    modifier(tag, req, value);
            });
    
    public static IConventionAction AddClass(this IConventionAction convention, string className) => 
        convention.Modify(tag => tag.Attributes.PrependAttr(HtmlAttr.Class, className));
    
    public static IConventionAction SetClass(this IConventionAction convention, string className) => 
        convention.Modify(tag => tag.Attributes.SetAttribute(HtmlAttr.Class, className));

    public static IConventionAction AppendAttr(
        this IConventionAction convention, 
        string attrName,
        string attrValue) =>
            convention.Modify(tag => tag.Attributes.Append(attrName, attrValue));
    
    public static IConventionAction SetAttr(
        this IConventionAction convention, 
        string attrName,
        string attrValue) =>
            convention.Modify(tag => tag.Attributes.SetAttribute(attrName, attrValue));
}