namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601190652)]
    public class M201601190652_Reset_all_ProcessedNotificationTracker : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("ProcessedNotificationTracker").AllRows();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}