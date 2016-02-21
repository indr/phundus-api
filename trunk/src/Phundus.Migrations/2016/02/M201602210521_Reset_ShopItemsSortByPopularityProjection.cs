namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602210521)]
    public class M201602210521_Reset_ShopItemsSortByPopularityProjection : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Es_Shop_ShopItemsSortByPopularityProjection");
            ResetProcessedNotififactionTracker("Phundus.Shop.Projections.ShopItemsSortByPopularityProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}