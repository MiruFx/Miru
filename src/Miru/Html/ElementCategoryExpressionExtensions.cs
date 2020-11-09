using HtmlTags.Conventions;

namespace Miru.Html
{
    public static class ElementCategoryExpressionExtensions
    {
        public static ElementActionExpression IfPropertyNameEnds(this ElementCategoryExpression expression, string text)
        {
            return expression.If(m => m.Accessor.Name.EndsWith(text));
        }

        public static ElementActionExpression IfPropertyNameStarts(this ElementCategoryExpression expression, string text)
        {
            return expression.If(m => m.Accessor.Name.StartsWith(text));
        }
        
        public static ElementActionExpression IfPropertyNameIs(this ElementCategoryExpression expression, string text)
        {
            return expression.If(m => m.Accessor.Name.Equals(text));
        }
    }
}