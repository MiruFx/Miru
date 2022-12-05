using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public class TagConventions : ITagConventions, IConventionAccessor
{
    public IList<Modifier> Modifiers { get; }

    public TagConventions()
    {
        Modifiers = new List<Modifier>();
    }

    public IConventionAction Always
    {
        get { return new ConventionAction((_, _) => true, Modifiers); }
    }

    public IConventionAction If(Func<ElementRequest, bool> condition) => 
        new ConventionAction((_, req) => condition(req) , Modifiers);

    public IConventionAction If(Func<TagHelperOutput, ElementRequest, bool> condition) => 
        new ConventionAction(condition, Modifiers);
}