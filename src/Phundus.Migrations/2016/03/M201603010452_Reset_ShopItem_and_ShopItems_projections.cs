namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603010464)]
    public class M201603010464_Reset_ShopItem_and_ShopItems_projections : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_Shop_ShopItemsSortByPopularityProjection");
            DeleteTable("Es_Shop_Items_Popularity");
            DeleteTable("Es_Shop_Item_Images");
            DeleteTable("Es_Shop_Item_Files");
            DeleteTable("Es_Shop_Item_Documents");
            DeleteTable("Es_Shop_Item");
            DeleteTable("Es_Shop_Items");

            DeleteTracker("ShopItemsSortByPopularityProjection");
            DeleteTracker("ShopItemPopularityProjection");
            DeleteTracker("ShopItemImagesProjection");
            DeleteTracker("ShopItemFilesProjection");
            DeleteTracker("ShopItemDocumentsProjection");
            DeleteTracker("ShopItemProjection");
            DeleteTracker("ShopItemsProjection");
        }
    }
}