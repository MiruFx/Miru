using System.Data;
using FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;

[Migration(202001290120)]
public class CreateUserfy : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
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
            .WithColumn("AccessFailedCount").AsInt32().NotNullable();

        Create.Table("Roles")
            .WithColumn("Id").AsInt64().PrimaryKey().NotNullable()
            .WithColumn("ConcurrencyStamp").AsString().Nullable()
            .WithColumn("Name").AsString(256).NotNullable()
            .WithColumn("NormalizedName").AsString(256).Nullable().Indexed();
        
        Create.Table("RoleClaims")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ClaimType").AsString().Nullable()
            .WithColumn("ClaimValue").AsString().Nullable()
            .WithColumn("RoleId").AsInt64().Indexed().ForeignKey("Roles", "Id").OnDelete(Rule.Cascade);
        
        Create.Table("UserClaims")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("ClaimType").AsString().Nullable()
            .WithColumn("ClaimValue").AsString().Nullable()
            .WithColumn("UserId").AsInt64().Indexed().ForeignKey("Users", "Id").OnDelete(Rule.Cascade);

        Create.Table("UserLogins")
            .WithColumn("LoginProvider").AsString().NotNullable().PrimaryKey()
            .WithColumn("ProviderKey").AsString().NotNullable().PrimaryKey()
            .WithColumn("ProviderDisplayName").AsString().Nullable()
            .WithColumn("UserId").AsInt64().Indexed().ForeignKey("Users", "Id").OnDelete(Rule.Cascade);

        Create.Table("UserRoles")
            .WithColumn("UserId").AsInt64().PrimaryKey().Indexed().ForeignKey("Users", "Id").OnDelete(Rule.Cascade)
            .WithColumn("RoleId").AsInt64().PrimaryKey().Indexed().ForeignKey("Roles", "Id").OnDelete(Rule.Cascade);

        Create.Table("UserTokens")
            .WithColumn("UserId").AsInt64().PrimaryKey().Indexed().ForeignKey("Users", "Id").OnDelete(Rule.Cascade)
            .WithColumn("LoginProvider").AsString().PrimaryKey()
            .WithColumn("Name").AsString().PrimaryKey()
            .WithColumn("Value").AsString().Nullable();
    }
}