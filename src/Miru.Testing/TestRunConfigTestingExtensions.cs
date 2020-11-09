namespace Miru.Testing
{
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
                _.Logout();
                
                _.ClearFabricator();
                _.ClearDatabase();
                _.ClearQueue();
            });
        }
    }
}