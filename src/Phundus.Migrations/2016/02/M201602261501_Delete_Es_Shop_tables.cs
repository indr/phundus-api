namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602261501)]
    public class M201602261501_Delete_Es_Shop_tables : MigrationBase
    {
        public override void Up()
        {
            DeleteTableAndResetTracker("Es_Shop_Item_Files", "FilesProjection");
            DeleteTableAndResetTracker("Es_Shop_Item_Images", "ImagesProjection");
            DeleteTableAndResetTracker("Es_Shop_ShopItemsSortByPopularityProjection", "ShopItemsSortByPopularityProjection");
            DeleteTableAndResetTracker("Es_Shop_ResultItems", "ResultItemsProjection");
            DeleteTableAndResetTracker("Es_Shop_Items", "ItemProjection");
            DeleteTableAndResetTracker("Es_Shop_ItemDetails", "DetailsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}