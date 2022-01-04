namespace Miru.Userfy
{
    public static class UserSessionExtensions
    {
        public static bool NotSignedIn(this IUserSession userSession)
        {
            return userSession.IsAuthenticated == false;
        }
    }
}