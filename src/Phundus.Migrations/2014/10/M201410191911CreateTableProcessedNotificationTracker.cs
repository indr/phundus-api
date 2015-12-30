namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201410191911)]
    public class M201410191911CreateTableProcessedNotificationTracker : MigrationBase
    {
        public override void Up()
        {
            Create.Table("ProcessedNotificationTracker")
                .WithColumn("TrackerId").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("ConcurrencyVersion").AsInt32().NotNullable()
                .WithColumn("TypeName").AsString(1024).NotNullable().Unique()
                .WithColumn("MostRecentProcessedNotificationId").AsInt64().Nullable();
        }

        public override void Down()
        {
        }
    }
}