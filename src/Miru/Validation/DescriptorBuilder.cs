using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace Miru.Validation
{
    public class DescriptorBuilder
    {
        private readonly IEnumerable<IPropertyValidator> _validators;

        public DescriptorBuilder(IEnumerable<IPropertyValidator> validators)
        {
            _validators = validators;
        }

        public bool Has<T>() => 
            _validators.OfType<T>().Any();
        
        public bool Has(Type type) => 
            _validators.Any(t => t.GetType().GetGenericTypeDefinition() == type);

        public T Get<T>() where T : IPropertyValidator
        {
            return _validators.OfType<T>().Single();
        }
        
        public object Get(Type type)
        {
            return _validators.SingleOrDefault(t => t.GetType().GetGenericTypeDefinition() == type);
        }
    }
}