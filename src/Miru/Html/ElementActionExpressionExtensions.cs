using System;
using HtmlTags;
using HtmlTags.Conventions;

namespace Miru.Html;

public static class ElementActionExpressionExtensions
{
    public static void ModifyTag(this ElementActionExpression expression, Action<HtmlTag> action)
    {
        expression.ModifyWith(modifier => action(modifier.CurrentTag));
    }
    
    public static ElementActionExpression Text(
        this ElementActionExpression expression, 
        string text)
    {
        expression.ModifyTag(tag => tag.Text(text));
        return expression;
    }
    
    public static ElementActionExpression Text<TValue>(
        this ElementActionExpression expression, 
        Func<TValue, string> func)
    {
        expression.ModifyWith(m =>
        {
            if (m.RawValue is TValue value)
            {
                m.CurrentTag.Text(func(value));
            }
        });
        
        return expression;
    }
    
    public static ElementActionExpression Placeholder(
        this ElementActionExpression expression, 
        string text)
    {
        expression.ModifyTag(tag => tag.Placeholder(text));
        return expression;
    }
    
    public static ElementActionExpression MaxLength(this ElementActionExpression expression, int maxLength)
    {
        expression.ModifyTag(tag => tag.MaxLength(maxLength));
        return expression;
    }
}