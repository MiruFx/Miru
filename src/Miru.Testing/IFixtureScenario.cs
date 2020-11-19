using System.Threading.Tasks;

namespace Miru.Testing
{
    public interface IFixtureScenario
    {
        Task BuildAsync(ITestFixture _);
    }
}