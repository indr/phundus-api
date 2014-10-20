namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201410200709)]
    public class M201410200709DeleteTableRmEventsAndEmptyTrackers : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Rm_Events").Exists())
                Delete.Table("Rm_Events");

            Delete.FromTable("ProcessedNotificationTracker").AllRows();
        }

        public override void Down()
        {
        }
    }
}