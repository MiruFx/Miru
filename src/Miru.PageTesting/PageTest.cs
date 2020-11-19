using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.PageTesting
{
    public abstract class PageTest : MiruTest, IPageTest
    {
        protected new readonly PageTestFixture _;
        
        protected PageTest()
        {
            base._.Get<PageTestingConfig>();
            
            _ = base._.Get<PageTestFixture>();
        }
        
#pragma warning disable 1998
        public virtual async Task GivenAsync()
#pragma warning restore 1998
        {
        }
        
        public virtual void Given()
        {
        }

        [OneTimeSetUp]
        public async Task PageTestOneTimeSetupAsync() => await GivenAsync();
        
        [OneTimeSetUp]
        public void PageTestOneTimeSetup() => Given();
    }
}