namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602280547)]
    public class M201602280547_Reset_Es_Shop_Orders : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_Shop_Orders_Lines");
            DeleteTable("Es_Shop_Orders");
            DeleteTracker("OrderProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}