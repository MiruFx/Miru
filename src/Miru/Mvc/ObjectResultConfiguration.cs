using System;
using System.Collections.Generic;

namespace Miru.Mvc
{
    public abstract class ObjectResultConfiguration
    {
        public readonly List<ObjectResultConfigExpression> Rules = new List<ObjectResultConfigExpression>();

        public ObjectResultConfigExpression When(Func<ObjectResultContext, bool> condition)
        {
            var rule = new ObjectResultConfigExpression
            {
                When = condition
            };
            
            Rules.Add(rule);

            return rule;
        }
    }
}