using FluentValidation;

namespace Miru.Mailing
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(x => x.FromAddress.EmailAddress).NotEmpty();

            RuleFor(x => x.ToAddresses).NotEmpty();
        }
    }
}