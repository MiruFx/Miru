using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202006172126)]
    public class CreateDonations : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Donations")
                .WithColumn("Id").AsId()
                .WithColumn("ProjectId").AsReference()
                .WithColumn("UserId").AsReference()
                .WithColumn("Amount").AsDecimal()
                .WithColumn("CreditCard").AsString(64)
                .WithTimeStamps();
        }
    }
}
