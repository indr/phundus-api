namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602201211)]
    public class M201602201211_Delete_table_Es_Shop_ResultItems : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_ShopItemsSortByPopularityProjection").Exists())
                Delete.Table("Es_Shop_ShopItemsSortByPopularityProjection");
            if (Schema.Table("Es_Shop_ResultItems").Exists())
                Delete.Table("Es_Shop_ResultItems");
            ResetTracker("Phundus.Shop.Projections.ShopItemsSortByPopularityProjection");
            ResetTracker("Phundus.Shop.Projections.ResultItemsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}