using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
// using Remotion.Linq.Parsing.ExpressionVisitors;

namespace Miru.Databases.EntityFramework
{
    // public static class EntityFrameworkFilterExtensions
    // {
    //     public static void HasQueryFilterFor<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> filterExpression)
    //     {
    //         foreach (var entityType in modelBuilder.Model.GetEntityTypes()
    //             .Where(e => typeof(TInterface).IsAssignableFrom(e.ClrType)))
    //         {
    //             modelBuilder
    //                 .Entity(entityType.ClrType)
    //                 .HasQueryFilter(ConvertFilterExpression(filterExpression, entityType.ClrType));
    //         }
    //     }
    //
    //     private static LambdaExpression ConvertFilterExpression<TInterface>(
    //         Expression<Func<TInterface, bool>> filterExpression,
    //         Type entityType)
    //     {
    //         var newParam = Expression.Parameter(entityType);
    //         var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);
    //
    //         return Expression.Lambda(newBody, newParam);
    //     }
    // }
}