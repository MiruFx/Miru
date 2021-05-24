using Miru.Mailing;
using Playground.Domain;

namespace Playground.Features.Accounts
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
                .Subject("Welcome To Playground")
                .Template("_Registered");
        }
    }
}
