using System.Threading.Tasks;

namespace Miru.Testing
{
    public interface IFixtureScenario
    {
        Task Build(ITestFixture _);
    }
}