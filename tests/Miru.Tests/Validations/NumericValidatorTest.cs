using FluentValidation;
using FluentValidation.TestHelper;
using Miru.Validation;
using NUnit.Framework;

namespace Miru.Tests.Validations
{
    public class NumericValidatorTest
    {
        [Test]
        public void Between_min_and_max()
        {
            var validator = new Validator();
    
            validator.ShouldHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 9 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 10 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 11 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 19 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 20 });
            validator.ShouldHaveValidationErrorFor(x => x.Between10And20, new Dto() { Between10And20 = 21 });
        }
        
        [Test]
        public void Minimum()
        {
            var validator = new Validator();
            
            validator.ShouldHaveValidationErrorFor(x => x.Min50, new Dto() { Min50 = 49 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Min50, new Dto() { Min50 = 50 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Min50, new Dto() { Min50 = 51 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Min50, new Dto() { Min50 = long.MaxValue });
        }
        
        [Test]
        public void Numeric()
        {
            var validator = new Validator();
            
            validator.ShouldNotHaveValidationErrorFor(x => x.Numeric, new Dto() { Numeric = 1 });
            validator.ShouldNotHaveValidationErrorFor(x => x.Numeric, new Dto() { Numeric = long.MinValue });
            validator.ShouldNotHaveValidationErrorFor(x => x.Numeric, new Dto() { Numeric = long.MaxValue });
        }
        
        public class Dto
        {
            public long Between10And20 { get; set; }
            public long Min50 { get; set; }
            public long Numeric { get; set; }
        }

        public class Validator : AbstractValidator<Dto>
        {
            public Validator()
            {
                RuleFor(m => m.Between10And20).Numeric(min: 10, max: 20);
                
                RuleFor(m => m.Min50).Numeric(min: 50);
                
                RuleFor(m => m.Numeric).Numeric();
            }
        }
    }
}