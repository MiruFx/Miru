using System.ComponentModel.DataAnnotations.Schema;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miru.Databases;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;
using Miru.Domain;
using Miru.Sqlite;

namespace Miru.Tests.Databases.Migrations;

public class DatabaseMigrationTest
{
    [Test]
    public void Should_run_migration_updating_schema()
    {
        // arrange
        using var test = new MiruCoreTest(services => services
            .Configure<DatabaseOptions>(x => x.ConnectionString = "DataSource={{ db_dir }}miru_test1.db")
            .AddMigrator<DatabaseMigrationTest>(x => x.AddSQLite())
            .AddEfCoreSqlite<FooDbContext>()).DisposableTestFixture;
        
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_test1.db").DeleteFileIfExists();
        
        // act
        test.Get<IDatabaseMigrator>().UpdateSchema();
        
        // assert
        test.WithDb<FooDbContext>(db => db.Posts.Any().ShouldBeFalse());
    }
    
    [Test]
    public void Should_run_migration_downgrading_schema()
    {
        // arrange
        using var test = new MiruCoreTest(services => services
            .Configure<DatabaseOptions>(x => x.ConnectionString = "DataSource={{ db_dir }}miru_test2.db")
            .AddMigrator<DatabaseMigrationTest>(x => x.AddSQLite())
            .AddEfCoreSqlite<FooDbContext>()).DisposableTestFixture;
        
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_test2.db").DeleteFileIfExists();
        
        test.Get<IDatabaseMigrator>().UpdateSchema();
        
        // act
        test.Get<IDatabaseMigrator>().DowngradeSchema();
        
        // assert
        Should.Throw<SqliteException>(() => test.WithDb<FooDbContext>(db => db.Posts.Any().ShouldBeFalse()));
    }
    
    [Test]
    public void Should_run_migration_with_different_connection_string_than_database_connection_string()
    {
        // arrange
        using var test = new MiruCoreTest(services => services
            .Configure<DatabaseOptions>(x => x.ConnectionString = "DataSource={{ db_dir }}miru_test3.db")
            .Configure<MigrationOptions>(x => x.ConnectionString = "DataSource={{ db_dir }}miru_migration_test.db")
            .AddMigrator<DatabaseMigrationTest>(x => x.AddSQLite())
            .AddEfCoreSqlite<FooDbContext>()).DisposableTestFixture;
        
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_test3.db").DeleteFileIfExists();
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_migration_test.db").DeleteFileIfExists();
        
        // act
        test.Get<IDatabaseMigrator>().UpdateSchema();
        
        // assert
        Should.Throw<SqliteException>(() => test.WithDb<FooDbContext>(db => db.Posts.Any().ShouldBeFalse()));
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_migration_test.db").ShouldExist();
        (test.Get<MiruSolution>().StorageDir / "db" / "miru_test3.db").ShouldBeEmpty();
    }
    
    [Table("Posts")]
    public class Post : Entity
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class FooDbContext : DbContext
    {
        public FooDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
    
    [Migration(202301061716)]
    public class CreatePosts : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Posts")
                .WithColumn("Id").AsId()
                .WithColumn("Title").AsString(64)
                .WithColumn("Body").AsString(256);
        }
    }
}

