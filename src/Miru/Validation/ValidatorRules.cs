using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace Miru.Validation
{
    public class ValidatorRules
    {
        private readonly IEnumerable<IPropertyValidator> _validators;

        public ValidatorRules(IEnumerable<IPropertyValidator> validators)
        {
            _validators = validators;
        }

        public bool Has<T>()
        {
            return _validators.OfType<T>().Any();
        }

        public bool Has(Type type)
        {
            return _validators.Any(t =>
            {
                var validatorType = t.GetType();

                return validatorType == type || 
                       validatorType.GetGenericTypeDefinition() == type ||
                       validatorType.Implements(type);
            });
        }

        public T Get<T>()
        {
            return _validators.OfType<T>().SingleOrDefault();
        }
        
        public object Get(Type type)
        {
            return _validators.SingleOrDefault(t =>
            {
                var validatorType = t.GetType();

                return validatorType == type || 
                       validatorType.GetGenericTypeDefinition() == type ||
                       validatorType.Implements(type);
            });
        }
    }
}