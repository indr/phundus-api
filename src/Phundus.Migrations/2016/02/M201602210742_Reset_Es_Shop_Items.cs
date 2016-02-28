namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602210743)]
    public class M201602210743_Reset_Es_Shop_Items : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_Item_Images").Exists())
                Delete.FromTable("Es_Shop_Item_Images").AllRows();
            if (Schema.Table("Es_Shop_Item_Files").Exists())
                Delete.FromTable("Es_Shop_Item_Files").AllRows();
            if (Schema.Table("Es_Shop_Items").Exists())
                Delete.FromTable("Es_Shop_Items").AllRows();
            
            ResetTracker("Phundus.Shop.Projections.ShopItemProjection");
            ResetTracker("Phundus.Shop.Projections.ShopItemImagesProjection");
            ResetTracker("Phundus.Shop.Projections.ShopItemFilesProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}