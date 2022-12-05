using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public class ConventionAction : IConventionAction
{
    protected readonly Func<TagHelperOutput, ElementRequest, bool> Condition;
    protected readonly IList<Modifier> Modifiers;

    public ConventionAction(Func<TagHelperOutput, ElementRequest, bool> condition, IList<Modifier> modifiers)
    {
        Condition = condition;
        Modifiers = modifiers;
    }

    public IConventionAction Modify(Action<TagHelperOutput, ElementRequest> modifier)
    {
        Modifiers.Add(new Modifier(Condition, modifier));
        return this;
    }
}