using System.Data;
using FluentMigrator;
using Miru.Behaviors.TimeStamp;
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
        
        Create.Table("Roles")
            .WithId()
            .WithColumn("ConcurrencyStamp").AsString().Nullable()
            .WithColumn("Name").AsString(256).NotNullable()
            .WithColumn("NormalizedName").AsString(256).Nullable().Indexed();
            
        Create.Table("RoleClaims")
            .WithId()
            .WithColumn("ClaimType").AsString().Nullable()
            .WithColumn("ClaimValue").AsString().Nullable()
            .WithColumn("RoleId").AsForeignKeyReference("Roles").Indexed();
        
        Create.Table("UserClaims")
            .WithId()
            .WithColumn("ClaimType").AsString().Nullable()
            .WithColumn("ClaimValue").AsString().Nullable()
            .WithColumn("UserId").AsForeignKeyReference("Users").Indexed();

        Create.Table("UserfyUserLogins")
            .WithColumn("LoginProvider").AsString().NotNullable().PrimaryKey()
            .WithColumn("ProviderKey").AsString().NotNullable().PrimaryKey()
            .WithColumn("ProviderDisplayName").AsString().Nullable()
            .WithColumn("UserId").AsForeignKeyReference("Users").Indexed();

        Create.Table("UserRoles")
            .WithColumn("UserId").AsForeignKeyReference("Users").Indexed()
            .WithColumn("RoleId").AsForeignKeyReference("Roles").Indexed();
        
        Create.Table("UserTokens")
            .WithColumn("UserId").AsForeignKeyReference("Users").Indexed()
            .WithColumn("LoginProvider").AsString().PrimaryKey()
            .WithColumn("Name").AsString().PrimaryKey()
            .WithColumn("Value").AsString().Nullable();    
    }
}