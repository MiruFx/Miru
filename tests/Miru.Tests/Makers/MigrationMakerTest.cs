using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class MigrationMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath / "Miru" / "Shopifu";
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Make_migration()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Migration("CreateTopup", "123");
            
            (_solutionDir / "src" / "Shopifu" / "Database" / "Migrations" / "123_CreateTopup.cs")
                .ShouldContain(
                    "namespace Shopifu.Database.Migrations",
                    $"[Migration(123)]",
                    "public class CreateTopup",
                    "Create.Table(\"TableName\")");
        }
        
        [Test]
        public void Make_migration_with_table_name()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Migration("CreateTeams", "456", table: "Teams");
            
            (_solutionDir / "src" / "Shopifu" / "Database" / "Migrations" / "456_CreateTeams.cs")
                .ShouldContain(
                    "namespace Shopifu.Database.Migrations",
                    $"[Migration(456)]",
                    "public class CreateTeams",
                    "Create.Table(\"Teams\")");
        }
        
        [Test]
        public void Make_migration_using_alter_table_template()
        {
            var m = new Maker(new MiruSolution(_solutionDir));
            
            m.Migration("AlterTeamsAddCategoryId", "456", table: "Teams");
            
            (_solutionDir / "src" / "Shopifu" / "Database" / "Migrations" / "456_AlterTeamsAddCategoryId.cs")
                .ShouldContain(
                    "namespace Shopifu.Database.Migrations",
                    $"[Migration(456)]",
                    "public class AlterTeamsAddCategoryId",
                    "Alter.Table(\"Teams\").AddColumn(\"ColumnName\");");
        }
    }
}
