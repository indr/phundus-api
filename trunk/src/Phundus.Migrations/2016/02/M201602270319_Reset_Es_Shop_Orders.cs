namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602270319)]
    public class M201602270319_Reset_Es_Shop_Orders : MigrationBase
    {
        public override void Up()
        {
            DeleteTableAndResetTracker("Es_Shop_Orders_Lines", "OrderProjection");
            DeleteTableAndResetTracker("Es_Shop_Orders", "OrderProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}