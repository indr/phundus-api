namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603182224)]
    public class M201603182224_Remove_ShopItem_and_ShopItems_projections : MigrationBase
    {
        public override void Up()
        {
            DeleteTracker("ShopItemProjection");
            DeleteTracker("ShopItemsProjection");

            DeleteTable("Es_Shop_Item_Documents");
            DeleteTable("Es_Shop_Item_Images");
            DeleteTable("Es_Shop_Item");

            DeleteTable("Es_Shop_Items_Popularity");
            DeleteTable("Es_Shop_Items");
        }
    }
}