using System;
using HtmlTags;
using HtmlTags.Conventions;

namespace Miru.Html
{
    public static class ElementActionExpressionExtensions
    {
        public static void ModifyTag(this ElementActionExpression expression, Action<HtmlTag> action)
        {
            expression.ModifyWith(modifier => action(modifier.CurrentTag));
        }
    }
}