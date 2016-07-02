namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602031143)]
    public class M201602031143_Reset_processed_notification_trackers : MigrationBase
    {
        public override void Up()
        {
            ResetAllProcessedNotificationTrackers();
            if (Schema.Table("Es_Dashboard_EventLog").Exists())
                Delete.FromTable("Es_Dashboard_EventLog").AllRows();
            Delete.FromTable("Es_IdentityAccess_Relationships").AllRows();
            Delete.FromTable("Es_Inventory_Articles").AllRows();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}