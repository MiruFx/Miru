using Pantanal.Domain;
using Miru.Mailing;

namespace Pantanal.Features.Accounts
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
            mail.To(_user.Email, _user.Email)
                .Subject("Welcome To Pantanal")
                .Template("_Registered");
        }
    }
}
