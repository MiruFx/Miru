using System.Threading.Tasks;
using Miru;
using Miru.PageTesting;
using Miru.Testing;
using Mong.Domain;

namespace Mong.PageTests.Fixtures
{
    public class TopupBasic : IFixtureScenario
    {
        public User User { get; set; }
        public Provider Provider { get; set; }
        
        public async Task Build(ITestFixture _)
        {
            User = _.Make<User>(m => m.HashedPassword = Hash.Create("123456"));
            Provider = _.Make<Provider>(m => m.Amounts = "10,20");

            await _.Save(User, Provider);
            
            _.LoginAs(User);
        }
    }
}