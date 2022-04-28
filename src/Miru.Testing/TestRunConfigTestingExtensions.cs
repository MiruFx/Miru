namespace Miru.Testing;

public static class TestRunConfigTestingExtensions
{
    public static void TestingDefault(this TestRunConfig run)
    {
        run.BeforeSuite(_ =>
        {
            _.MigrateDatabase();
        });
    
        run.BeforeCase(_ =>
        {
            _.ClearFabricator();
        });
            
        run.BeforeCase<IIntegratedTest>(_ =>
        {
            _.Logout();
            _.ClearDatabase();
            _.ClearQueue();
        });
    }
}