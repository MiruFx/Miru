using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Mong.Database.Migrations
{
    [Migration(201911071845)]
    public class CreateTopup : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Topups")
                .WithId()
                .WithColumn("UserId").AsReference()

                .WithColumn("ProviderId").AsReference()
                
                .WithColumn("PhoneNumber").AsString(64)
                .WithColumn("Amount").AsDecimal()

                .WithColumn("Email").AsString(128)
                .WithColumn("Name").AsString(128)

                .WithColumn("PaymentId").AsString(64)
                .WithColumn("PaidAt").AsDateTime()
                .WithColumn("Status").AsByte()

                .WithTimeStamps();
        }
    }
}