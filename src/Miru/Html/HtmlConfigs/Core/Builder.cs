using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public class Builder
{
    public Func<ModelExpression, bool> Condition { get; }
    public Action<ModelExpression, TagHelperOutput> BuilderFunc { get; }
    public Action<ModelExpression, IConventionPipeline, TagHelperOutput> BuilderFuncPipeline { get; }
    
    public Builder(Func<ModelExpression, bool> condition, Action<ModelExpression, TagHelperOutput> builder)
    {
        Condition = condition;
        BuilderFunc = builder;
    }

    public Builder(
        Func<ModelExpression, bool> condition, 
        Action<ModelExpression, IConventionPipeline, TagHelperOutput> builder)
    {
        Condition = condition;
        BuilderFuncPipeline = builder;
    }
}