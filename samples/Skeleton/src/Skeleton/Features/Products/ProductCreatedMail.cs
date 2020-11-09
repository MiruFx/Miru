using Miru.Mailing;
using Skeleton.Domain;

namespace Skeleton.Features.Products
{
    public class ProductCreatedMail : Mailable
    {
        private readonly User _user;

        public ProductCreatedMail(User user)
        {
            _user = user;
        }

        public override void Build(Email mail)
        {
            mail.To(_user.Email, _user.Name)
                .Subject("Email Subject")
                .Template();
        }
    }
}