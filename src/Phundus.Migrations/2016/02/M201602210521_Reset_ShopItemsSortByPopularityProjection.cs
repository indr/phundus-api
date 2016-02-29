namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602210521)]
    public class M201602210521_Reset_ShopItemsSortByPopularityProjection : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_ShopItemsSortByPopularityProjection").Exists())
                Delete.Table("Es_Shop_ShopItemsSortByPopularityProjection");
            DeleteTracker("Phundus.Shop.Projections.ShopItemsSortByPopularityProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}