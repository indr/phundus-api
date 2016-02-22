namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602221138)]
    public class M201602221139_Add_column_RecievesEmailNotifications : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_IdentityAccess_Membership")
                .AddColumn("RecievesEmailNotifications")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false);

            Execute.Sql(@"UPDATE [Dm_IdentityAccess_Membership] SET [RecievesEmailNotifications] = 1 WHERE [Role] = 2");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}