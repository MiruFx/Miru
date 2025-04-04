using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Hosting;
using Miru.SqlServer;

namespace Miru.Tests.EntityFramework
{
    public class EntityFrameworkDatabaseCreatorTest
    {
        [Test]
        [Ignore("Because I can")]
        public async Task Should_create_database_only()
        {
            var app = MiruHost.CreateMiruHost()
                .ConfigureServices(services =>
                {
                    services.AddEfCoreSqlServer<TestDbContext>();

                    var appSettings = new DatabaseOptions()
                    {
                        ConnectionString =
                            "Data Source=.\\sqlexpress;Initial Catalog=miru_test;Integrated Security=SSPI;"
                    };

                    services.AddSingleton(appSettings);
                })
                .BuildApp();

            await app.Get<IDatabaseCreator>().Create();
        }
        
        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions options) : base(options)
            {
            }
        }
    }
}