using Miru.Mailing;

namespace Miru.Userfy
{
    public static class EmailExtensions
    {
        public static Email To<TUser>(this Email emailData, TUser user) where TUser : UserfyUser
        {
            emailData.To(user.Email);
            
            return emailData;
        }
    }
}