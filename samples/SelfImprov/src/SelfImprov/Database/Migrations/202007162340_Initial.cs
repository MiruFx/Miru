using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace SelfImprov.Database.Migrations
{
    [Migration(202007162340)]
    public class Initial : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Areas")
                .WithId()
                .WithColumn("UserId").AsReference()
                .WithColumn("Name").AsString(64).Nullable()
                .WithColumn("IsInactive").AsBoolean().WithDefaultValue(false).Indexed()
                .WithTimeStamps();
            
            Create.Table("Goals")
                .WithId()
                .WithColumn("AreaId").AsReference()
                .WithColumn("UserId").AsReference()
                .WithColumn("Name").AsString(64).Nullable()
                .WithColumn("IsInactive").AsBoolean().WithDefaultValue(false).Indexed()
                .WithTimeStamps();

            Create.Table("Achievements")
                .WithId()
                .WithColumn("IterationId").AsReference()
                .WithColumn("GoalId").AsReference()
                .WithColumn("Achieved").AsBoolean()
                .WithTimeStamps();
            
            Create.Table("Iterations")
                .WithColumn("Id").AsId()
                .WithColumn("Number").AsInt32()
                .WithColumn("UserId").AsReference()
                .WithTimeStamps();
        }
    }
}
