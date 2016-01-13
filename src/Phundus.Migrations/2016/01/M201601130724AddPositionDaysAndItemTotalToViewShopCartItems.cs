namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601130724)]
    public class M201601130724AddPositionDaysAndItemTotalToViewShopCartItems : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_CartItems]");
            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_CartItems]
AS
SELECT     CartItemId, CartItemGuid, CartId, CartGuid, Position, FromUtc, ToUtc, Days, Quantity, ItemTotal, Article_ArticleId, Article_Name, Article_UnitPricePerWeek, Article_Owner_OwnerId, 
                      Article_Owner_Name
FROM         dbo.Dm_Shop_CartItem");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}