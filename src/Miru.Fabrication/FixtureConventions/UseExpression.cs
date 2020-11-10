using System;

namespace Miru.Fabrication.FixtureConventions
{
    public class UseExpression
    {
        private readonly Func<object> _use;
        private readonly ConventionExpression _conventionExpression;

        public UseExpression(Func<object> use, ConventionExpression conventionExpression)
        {
            _use = use;
            _conventionExpression = conventionExpression;
        }

        public void If(Action<ConventionExpression> @if)
        {
            @if(_conventionExpression);

            foreach (var expression in _conventionExpression.Expressions)
            {
                expression.Use(_use);
            }
        }
    }
}