using System.Data;
using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;

[Migration(202001290120)]
public class CreateUsers : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithId()
            .WithColumn("Name").AsString(256)
            .WithColumn("UserName").AsString(256).Nullable()
            .WithColumn("NormalizedUserName").AsString(256).Nullable()
            .WithColumn("Email").AsString(256).Nullable()
            .WithColumn("NormalizedEmail").AsString(256).Nullable()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("PasswordHash").AsString().Nullable()
            .WithColumn("SecurityStamp").AsString().Nullable()
            .WithColumn("ConcurrencyStamp").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString().Nullable()
            .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
            .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
            .WithColumn("LockoutEnd").AsDateTime2().Nullable()
            .WithColumn("LockoutEnabled").AsBoolean().Nullable()
            .WithColumn("AccessFailedCount").AsInt32().NotNullable()
            .WithTimeStamps();
    }
}