using System;
using FluentValidation.Validators;

namespace Miru.Validation
{
    public class NumericValidator : PropertyValidator 
    {
        private readonly long _min;
        private readonly long _max;

        public NumericValidator() : base("'{PropertyName}' must be a number.")
        {
            _min = long.MinValue;
            _max = long.MaxValue;
        }
        
        public NumericValidator(long min) : 
            base($"'{{PropertyName}}' must be a number higher than {min}.")
        {
            _min = min;
            _max = long.MaxValue;
        }
        
        public NumericValidator(long min, long max) : 
            base($"'{{PropertyName}}' must be a number between {min} and {max}.")
        {
            _min = min;
            _max = max;
        }

        protected override bool IsValid(PropertyValidatorContext context) 
        {
            var value = context.PropertyValue as string;

            if (value == null)
                return true;

            if (long.TryParse(value, out var number) == false)
                return false;

            if (number >= _min && number <= _max)
                return true;

            return false;
        }
    }
}