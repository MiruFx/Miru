using FluentValidation.Validators;

namespace Miru.Validation
{
    public class NumericValidator : PropertyValidator 
    {
        private readonly long _min;
        private readonly long _max;

        public NumericValidator()
        {
            _min = long.MinValue;
            _max = long.MaxValue;
        }
        
        public NumericValidator(long min)
        {
            _min = min;
            _max = long.MaxValue;
        }
        
        public NumericValidator(long min, long max)
        {
            _min = min;
            _max = max;
        }

        protected override string GetDefaultMessageTemplate()
        {
            if (_min == default && _max == default)
                return "'{PropertyName}' must be a number.";
            
            if (_min != default && _max == default)
                return "'{{PropertyName}}' must be a number higher than {min}.";
            
            return $"'{{PropertyName}}' must be a number between {_min} and {_max}.";
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