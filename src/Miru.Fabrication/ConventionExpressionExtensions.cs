using System;
using System.Linq.Expressions;
using Miru.Fabrication.FixtureConventions;

namespace Miru.Fabrication
{
    public static class ConventionExpressionExtensions
    {
        public static IfFilterExpression IfPropertyIs<TClass, TProperty>(
            this ConventionExpression conventionExpression, 
            Expression<Func<TClass, TProperty>> property)
        {
            var accessor = Baseline.Reflection.ReflectionHelper.GetAccessor(property);
            
            return conventionExpression.IfProperty(p => p.DeclaringType == accessor.OwnerType && 
                                                        p.PropertyType == accessor.PropertyType);
        }
    }
}