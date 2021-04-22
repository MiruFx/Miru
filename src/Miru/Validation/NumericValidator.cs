using FluentValidation;
using FluentValidation.Validators;

namespace Miru.Validation
{
    public class NumericValidator<T> : PropertyValidator<T,long> 
    {
        private readonly long _min;
        private readonly long _max;

        public override string Name => "NumericValidator";
        
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

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            if (_min == default && _max == default)
                return "'{PropertyName}' must be a number.";
            
            if (_min != default && _max == default)
                return "'{{PropertyName}}' must be a number higher than {min}.";
            
            return $"'{{PropertyName}}' must be a number between {_min} and {_max}.";
        }

        public override bool IsValid(ValidationContext<T> context, long value)
        {
            if (value >= _min && value <= _max)
                return true;

            return false;
        }
    }
}