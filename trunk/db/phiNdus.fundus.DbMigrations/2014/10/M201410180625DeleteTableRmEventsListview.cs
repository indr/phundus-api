namespace phiNdus.fundus.DbMigrations
{
    using FluentMigrator;

    [Migration(201410180625)]
    public class M201410180625DeleteTableRmEventsListview : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Rm_EventsListView").Exists())
                Delete.Table("Rm_EventsListView");
        }

        public override void Down()
        {
        }
    }
}