namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602261742)]
    public class M201602261742_Delete_Es_Shop_Orders_and_Es_Shop_OrderItems : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_OrderItems").Exists())
                Delete.Table("Es_Shop_OrderItems");
            if (Schema.Table("Es_Shop_Orders_Lines").Exists())
                Delete.Table("Es_Shop_Orders_Lines");
            
            DeleteTableAndTracker("Es_Shop_Orders", "OrderProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}