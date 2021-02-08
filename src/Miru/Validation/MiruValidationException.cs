using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Miru.Validation
{
    public class MiruValidationException : ValidationException
    {
        public object Model { get; private set; }
        
        public MiruValidationException(object model, IEnumerable<ValidationFailure> errors) : base(errors)
        {
            Model = model;
        }
    }
}