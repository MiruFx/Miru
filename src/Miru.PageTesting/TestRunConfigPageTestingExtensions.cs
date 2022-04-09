using Miru.Testing;

namespace Miru.PageTesting;

public static class TestRunConfigPageTestingExtensions
{
    public static void PageTestingDefault(this TestRunConfig run)
    {
        run.BeforeSuite(_ =>
        {
            _.MigrateDatabase();
            _.StartServer();
        });
            
        run.BeforeCase(_ =>
        {
            _.Logout();

            _.ClearDatabase();
            _.ClearFabricator();
        });
            
        run.AfterSuite(_ =>
        {
            _.QuitBrowser();
            _.StopServer();
        });
    }
}