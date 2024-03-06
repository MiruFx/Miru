namespace Miru.Testing
{
    public interface IFixtureScenario
    {
        Task BuildAsync(ITestFixture _);
    }
}