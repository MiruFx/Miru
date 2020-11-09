using System;
using FluentMigrator;
using Miru;
using Miru.Databases.Migrations.FluentMigrator;

namespace Mong.Database.Migrations
{
    [Migration(201911061800)]
    public class CreateUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64)
                .WithColumn("Email").AsString(255)
                .WithColumn("HashedPassword").AsString(128)

                .WithTimeStamps()
                
                // CanBeAdmin
                .WithColumn("IsAdmin").AsBoolean()
                
                // Recoverable
                .WithColumn("ResetPasswordToken").AsString(64).Nullable()
                .WithColumn("ResetPasswordSentAt").AsDateTime().Nullable()

                // Confirmable
                .WithColumn("ConfirmationToken").AsString(64).Nullable()
                .WithColumn("ConfirmationSentAt").AsDateTime().Nullable()
                .WithColumn("ConfirmedAt").AsDateTime().Nullable()
            
                // Blockable
                .WithColumn("BlockedAt").AsDateTime().Nullable();
        }
    }
}