using Miru.Mailing;
using Skeleton.Domain;

namespace Skeleton.Features.Accounts
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
                .Subject("Welcome To Skeleton")
                .Template();
        }
    }
}