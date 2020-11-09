using System.Threading.Tasks;
using NUnit.Framework;

namespace Miru.Testing
{
    public abstract class OneCaseFeatureTest : MiruTest, ICasePerFeatureTest
    {
#pragma warning disable 1998
        public virtual async Task Given()
#pragma warning restore 1998
        {
        }
        
        public virtual void GivenSync()
        {
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() => await Given();
        
        [OneTimeSetUp]
        public void OneTimeSetupSync() => GivenSync();
    }
}