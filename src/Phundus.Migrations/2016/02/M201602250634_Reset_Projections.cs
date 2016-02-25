namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602250635)]
    public class M201602250635_Reset_Projections : MigrationBase
    {
        public override void Up()
        {
            EmptyTableAndResetTracker("Es_Inventory_Articles_Actions", "ArticleActionsProjection");
            DeleteTableAndResetTracker("Es_Inventory_Articles", "ArticlesProjection");
            EmptyTableAndResetTracker("Es_Dashboard_EventLog", "EventLogProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}