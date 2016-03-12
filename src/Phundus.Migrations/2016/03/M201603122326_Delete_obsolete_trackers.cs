namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603122326)]
    public class M201603122326_Delete_obsolete_trackers : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [ProcessedNotificationTracker] WHERE [TypeName] LIKE '%IProjectionProxy' OR [TypeName] LIKE '%ArticlesActionsProjection'");
        }
    }
}