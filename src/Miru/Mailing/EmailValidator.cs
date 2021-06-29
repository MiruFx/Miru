using FluentValidation;

namespace Miru.Mailing
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
        }
    }
}