using Miru.Mailing;
using Mong.Domain;

namespace Mong.Features.Accounts
{
    // #mailable
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
                .Subject("Activate Your Mong Account")
                .Template(_user);
        }
    }
    // #mailable
}