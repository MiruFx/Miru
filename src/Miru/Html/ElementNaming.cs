using System;
using System.Linq;
using System.Linq.Expressions;
using HtmlTags.Reflection;
using Humanizer;
using Miru.Core;
using Miru.Domain;

namespace Miru.Html
{
    public class ElementNaming
    {
        private static readonly Func<string, bool> IsCollectionIndexer = x => x.StartsWith("[") && x.EndsWith("]");
        
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
            if (model is IEntity entity)
                return $"{model.GetType().Name.ToKebabCase()}-{entity.Id}";
                
            return $"{model.GetType().FeatureName().ToKebabCase()}-{model.GetHashCode()}";
        }
        
        public string Id(Type type)
        {
            return $"{type.FeatureName().ToKebabCase()}";
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
}