using System.Collections.Generic;
using System.Linq;
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

        public bool Has<T>()
        {
            return _validators.OfType<T>().Any();
        }

        public T Get<T>() where T : IPropertyValidator
        {
            return _validators.OfType<T>().Single();
        }
    }
}