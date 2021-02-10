using Miru.Mailing;
using SelfImprov.Domain;

namespace SelfImprov.Features.Accounts
{
    public class AccountRegisteredMail : Mailable
    {
        private readonly User _user;

        public AccountRegisteredMail(User user)
        {
            _user = user;
        }

        public override void Build(Email mail)
        {
            mail.To(_user.Email, _user.Name)
                .Subject("Welcome To SelfImprov")
                .Template(string.Empty);
        }
    }
}
