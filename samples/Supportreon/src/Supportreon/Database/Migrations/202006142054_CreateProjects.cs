using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202006142054)]
    public class CreateProjects : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Projects")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64)
                .WithColumn("Description").AsString(4000)
                .WithColumn("MinimumDonation").AsDecimal()
                .WithColumn("UserId").AsReference()
                .WithColumn("CategoryId").AsReference()
                .WithColumn("Goal").AsDecimal().Nullable()
                .WithColumn("EndDate").AsDate().Nullable()
                .WithColumn("TotalDonations").AsInt64()
                .WithColumn("TotalAmount").AsDecimal()
                .WithTimeStamps();
        }
    }
}
