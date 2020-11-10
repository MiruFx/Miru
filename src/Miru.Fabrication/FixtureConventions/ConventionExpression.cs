using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Bogus;

namespace Miru.Fabrication.FixtureConventions
{
    public class ConventionExpression
    {
        private readonly Faker _faker;
        private readonly Stack<IfFilterExpression> _expressions = new Stack<IfFilterExpression>();

        public IEnumerable<IfFilterExpression> Expressions => _expressions;

        public ConventionExpression(Faker faker)
        {
            _faker = faker;
        }

        public IfFilterExpression IfProperty(Expression<Func<PropertyInfo, bool>> whenProperty)
        {
            var expression = new IfFilterExpression(whenProperty, _faker);
            
            _expressions.Push(expression);
            
            return expression;
        }
        
        public IfFilterExpression IfClass(Expression<Func<TypeInfo, bool>> whenClass)
        {
            var expression = new IfFilterExpression(whenClass, _faker);
            
            _expressions.Push(expression);
            
            return expression;
        }

        public UseExpression Use(Func<Faker, object> useFaker)
        {
            return new UseExpression(() => useFaker(_faker), this);
        }
        
        public UseExpression Use(object useValue)
        {
            return new UseExpression(() => useValue, this);
        }
    }
}