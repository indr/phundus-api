namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201410192132)]
    public class M201410192132DeleteTableRmEventsListView : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Rm_EventsListView").Exists())
                Delete.Table("Rm_EventsListView");

            Delete.FromTable("ProcessedNotificationTracker").AllRows();
        }

        public override void Down()
        {
        }
    }
}