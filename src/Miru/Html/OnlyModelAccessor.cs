using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using HtmlTags.Reflection;

namespace Miru.Html
{
    public class OnlyModelAccessor : Accessor
    {
        private readonly object _model;

        public OnlyModelAccessor(object model)
        {
            _model = model;
        }

        public void SetValue(object target, object propertyValue)
        {
        }

        public object GetValue(object target)
        {
            return _model;
        }

        public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
        {
            return this;
        }

        public Expression<Func<T, object>> ToExpression<T>()
        {
            return null;
        }

        public Accessor Prepend(PropertyInfo property)
        {
            return this;
        }

        public IEnumerable<IValueGetter> Getters()
        {
            return new List<IValueGetter>();
        }

        public Type PropertyType => _model.GetType();

        public PropertyInfo InnerProperty => null;
        
        public Type DeclaringType => _model?.GetType();
        
        public string Name => _model.GetType().Name;

        public Type OwnerType => _model.GetType();

        public string[] PropertyNames => new[] { Name };
    }
}