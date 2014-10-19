namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201410192132)]
    public class M201410192132ClearRmEventsListView : MigrationBase
    {
        public override void Up()
        {
            EmptyTableAndResetTracker("Rm_EventsListView", "Phundus.Core.Dashboard.Querying.EventsListViewDao");
        }

        private void EmptyTableAndResetTracker(string tableName, string trackerTypeName)
        {
            Delete.FromTable(tableName).AllRows();
            Update.Table("ProcessedNotificationTracker").Set(new {MostRecentProcessedNotificationId = 0}).Where(new{TypeName = trackerTypeName});
        }

        public override void Down()
        {
        }
    }
}