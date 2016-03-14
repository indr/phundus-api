namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602261502)]
    public class M201602261502_Delete_Es_Shop_tables : MigrationBase
    {
        public override void Up()
        {
            DeleteTableAndTracker("Es_Shop_Item_Files", "FilesProjection");
            DeleteTableAndTracker("Es_Shop_Item_Images", "ImagesProjection");
            DeleteTableAndTracker("Es_Shop_ShopItemsSortByPopularityProjection", "ShopItemsSortByPopularityProjection");
            DeleteTableAndTracker("Es_Shop_ResultItems", "ResultItemsProjection");
            DeleteTableAndTracker("Es_Shop_Items", "ItemProjection");
            DeleteTableAndTracker("Es_Shop_ItemDetails", "DetailsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}