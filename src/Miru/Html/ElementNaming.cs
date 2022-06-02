using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using HtmlTags.Reflection;
using Humanizer;
using Miru.Core;
using Miru.Domain;

namespace Miru.Html;

public class ElementNaming
{
    private static readonly Func<string, bool> IsCollectionIndexer = x => x.StartsWith("[") && x.EndsWith("]");
        
    private static readonly Regex IdRegex = new(@"[\.\[\]]", RegexOptions.Compiled);

    public static string BuildId(string propertyName) 
        => IdRegex.Replace(propertyName, "_");
        
    public string Form<TModel>(TModel model)
    {
        return Form(model.GetType());
    }
        
    public string Form(Type type)
    {
        return $"{type.FeatureName().ToKebabCase()}-form";
    }

    public string Display(Type type)
    {
        return type.FeatureName().ToKebabCase();
    }
        
    public string Input<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
    {
        return IdFromExpression(property);
    }

    public string DisplayLabel<TModel, TProperty>(Expression<Func<TModel,TProperty>> property)
    {
        return IdFromExpression(property);
    }
        
    public string Id<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
        return Id(IdFromExpression(expression));
    }
        
    public string Id<TModel>(TModel model)
    {
        if (model is IHasId identifiable)
        {
            return $"{model.GetType().Name.ToKebabCase()}_{identifiable.Id}";
        }

        if (model is IEnumerable)
        {
            return Id(model.GetType());
        }

        return $"{model.GetType().FeatureName().ToKebabCase()}_@{model.GetHashCode()}";
    }
        
    public string Id(Type type)
    {
        if (type.ImplementsEnumerableOfSomething())
        {
            var genericArgumentType = type.GenericTypeArguments.First();
            
            return Id(genericArgumentType);
        }
        
        return type.FeatureName().ToKebabCase();
    }
        
    public string Link(object @for)
    {
        return $"{@for.GetType().FeatureName().ToKebabCase()}-link";
    }
        
    private string IdFromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
    {
        return property
            .ToAccessor()
            .PropertyNames
            .Aggregate((x, y) => string.Format(IsCollectionIndexer(y) ? "{0}{1}" : "{0}.{1}", x, y));
    }
}