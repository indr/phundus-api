namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201602210628)]
    public class M201602210628_Delete_stored_order_events_with_empty_serialization : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [StoredEvents] WHERE [TypeName] LIKE '%Order%' AND [Serialization] = 0");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}