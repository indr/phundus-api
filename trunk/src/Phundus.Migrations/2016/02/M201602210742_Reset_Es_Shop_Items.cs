namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602210743)]
    public class M201602210743_Reset_Es_Shop_Items : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("Es_Shop_Item_Images").AllRows();
            Delete.FromTable("Es_Shop_Item_Files").AllRows();
            Delete.FromTable("Es_Shop_Items").AllRows();
            
            ResetProcessedNotififactionTracker("Phundus.Shop.Projections.ShopItemProjection");
            ResetProcessedNotififactionTracker("Phundus.Shop.Projections.ShopItemImagesProjection");
            ResetProcessedNotififactionTracker("Phundus.Shop.Projections.ShopItemFilesProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}