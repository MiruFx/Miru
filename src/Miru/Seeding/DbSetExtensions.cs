using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Miru.Seeding;

public static class DbSetExtensions
{
    public static TEntity SeedBy<TEntity>(
        this DbSet<TEntity> set, 
        Expression<Func<TEntity, object>> seedBy, 
        Action<TEntity> setEntityAction) where TEntity : class, new()
    {
        var entity = new TEntity();

        setEntityAction(entity);
        
        var identifyingProperties = GetProperties(seedBy).ToList();
        
        var parameter = Expression.Parameter(typeof(TEntity));
        
        var matches = identifyingProperties.Select(pi => Expression.Equal(
            Expression.Property(parameter, pi.Name), 
            Expression.Constant(pi.GetValue(entity, null))));
        
        var matchExpression = matches.Aggregate<BinaryExpression, Expression>(
            null, 
            (agg, v) => (agg == null) ? v : Expression.AndAlso(agg, v));

        var predicate = Expression.Lambda<Func<TEntity, bool>>(matchExpression, parameter);

        var fetchedEntity = set.SingleOrDefault(predicate);
        
        if (fetchedEntity == null)
        {
            set.Add(entity);
            return entity;
        }
        else
        {
            setEntityAction(fetchedEntity);
            return fetchedEntity;
        }
    }

    private static IEnumerable<PropertyInfo> GetProperties<T>(Expression<Func<T, object>> exp) where T : class
    {
        var type = typeof(T);
        var properties = new List<PropertyInfo>();

        if (exp.Body.NodeType == ExpressionType.MemberAccess)
        {
            if (exp.Body is MemberExpression memExp)
                properties.Add(type.GetProperty(memExp.Member.Name));
        }
        else if (exp.Body.NodeType == ExpressionType.Convert)
        {
            if (exp.Body is UnaryExpression { Operand: MemberExpression propExp }) 
                properties.Add(type.GetProperty(propExp.Member.Name));
        }
        else if (exp.Body.NodeType == ExpressionType.New)
        {
            if (exp.Body is NewExpression newExp)
                properties.AddRange(newExp.Members.Select(x => type.GetProperty(x.Name)));
        }

        return properties.OfType<PropertyInfo>();
    }
}