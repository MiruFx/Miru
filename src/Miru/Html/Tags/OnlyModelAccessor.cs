using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Baseline.Reflection;

namespace Miru.Html.Tags;

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

    public object GetValue(object target) => _model;

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression) => this;

    public Expression<Func<T, object>> ToExpression<T>() => null;

    public Accessor Prepend(PropertyInfo property) => this;

    public IEnumerable<IValueGetter> Getters() => Enumerable.Empty<IValueGetter>();

    public string FieldName => Name;

    public Type PropertyType => null;

    public PropertyInfo InnerProperty => null;
        
    public Type DeclaringType => _model?.GetType();
        
    public string Name => _model != null ? _model.GetType().Name : string.Empty;

    public Type OwnerType => _model.GetType();

    public string[] PropertyNames => new[] { Name };
}