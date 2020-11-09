using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Miru.Mvc
{
    public abstract class ExceptionResultConfiguration
    {
        public readonly List<ExceptionResultExpression> Rules = new List<ExceptionResultExpression>();

        public ExceptionResultExpression When(Func<ExceptionResultContext, bool> condition)
        {
            var rule = new ExceptionResultExpression
            {
                When = condition
            };
            
            Rules.Add(rule);

            return rule;
        }
    }
}