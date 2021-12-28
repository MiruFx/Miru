using System;
using System.Linq;
using System.Linq.Expressions;
using Baseline.Reflection;

namespace Miru.Fabrication.FixtureConventions;

public static class ConventionExpressionExtensions
{
    public static IfFilterExpression IfPropertyNameIs(
        this ConventionExpression conventionExpression, string propertyName)
    {
        return conventionExpression.IfProperty(p => p.Name == propertyName);
    }
        
    public static IfFilterExpression IfPropertyNameStarts(
        this ConventionExpression conventionExpression, string propertyNameStart)
    {
        return conventionExpression.IfProperty(p => p.Name.StartsWith(propertyNameStart));
    }
        
    public static IfFilterExpression IfPropertyNameEnds(
        this ConventionExpression conventionExpression, string propertyNameEnd)
    {
        return conventionExpression.IfProperty(p => p.Name.EndsWith(propertyNameEnd));
    }
        
    public static IfFilterExpression IfPropertyTypeIs<TType>(this ConventionExpression conventionExpression)
    {
        return conventionExpression.IfProperty(p => p.PropertyType == typeof(TType));
    }
        
    public static IfFilterExpression IfPropertyImplements<TType>(this ConventionExpression conventionExpression)
    {
        return conventionExpression.IfProperty(p => p.PropertyType.Implements<TType>());
    }
        
    public static IfFilterExpression IfPropertyImplements(
        this ConventionExpression conventionExpression,
        Type typeImplemented)
    {
        return conventionExpression.IfProperty(p => 
            p.PropertyType.Implements(typeImplemented) || 
            p.PropertyType.ImplementsGenericOf(typeImplemented));
    }
        
    public static IfFilterExpression IfPropertyImplementsEnumerableOf<TType>(
        this ConventionExpression conventionExpression)
    {
        return conventionExpression.IfProperty(p => p.PropertyType.ImplementsEnumerableOf<TType>());
    }
        
    public static IfFilterExpression IfPropertyIs<TType>(
        this ConventionExpression conventionExpression,
        Expression<Func<TType, object>> expression) where TType : class
    {
        return conventionExpression.IfProperty(p => 
            p.DeclaringType == typeof(TType) && 
            p.Name == expression.ToAccessor().Name);
    }
        
    public static IfFilterExpression IfClassIs<TType>(this ConventionExpression conventionExpression)
    {
        return conventionExpression.IfClass(t => t == typeof(TType));
    }
        
    public static IfFilterExpression IfClassImplements<TType>(this ConventionExpression conventionExpression)
    {
        return conventionExpression.IfClass(t => t.Implements<TType>());
    }
        
    public static IfFilterExpression IfPropertyNameContains(
        this ConventionExpression conventionExpression, 
        params string[] propertyNames)
    {
        return conventionExpression.IfProperty(p => 
            propertyNames.Any(propertyName => p.Name.Contains(propertyName)));
    }
    
    public static IfFilterExpression IfPropertyNameContains<TType>(
        this ConventionExpression conventionExpression, 
        params string[] propertyNames)
    {
        return conventionExpression.IfProperty(p => 
            p.PropertyType == typeof(TType) && propertyNames.Any(propertyName => p.Name.Contains(propertyName)));
    }
}