namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602080815)]
    public class M201602080815_Drop_Table_Es_Shop_Item : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_ItemFiles").Exists())
                Delete.Table("Es_Shop_ItemFiles");
            if (Schema.Table("Es_Shop_ItemImages").Exists())
                Delete.Table("Es_Shop_ItemImages");
            if (Schema.Table("Es_Shop_Item").Exists())
                Delete.Table("Es_Shop_Item");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(201602080812)]
    public class M201602080812_Drop_Table_Es_Inventory_Articles : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Es_Inventory_Articles");
            ResetProcessedNotififactionTracker(@"Phundus.Inventory.Projections.ArticlesProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}