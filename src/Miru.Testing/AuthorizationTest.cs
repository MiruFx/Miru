using NUnit.Framework;

namespace Miru.Testing
{
    public abstract class AuthorizationTest : OneCaseFeatureTest
    {
        [OneTimeSetUp]
        public void AuthorizationOneTimeSetup()
        {
            _.Logout();
        }
    }
}