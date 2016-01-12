namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601130007)]
    public class M201601130007CreateViewsShopCartsAndShopCartItems : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_Carts]
AS
SELECT     CartId, CartGuid, UserId, UserGuid
FROM         dbo.Dm_Shop_Cart");

            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_CartItems]
AS
SELECT     CartItemId, CartItemGuid, CartId, CartGuid, FromUtc, ToUtc, Quantity, Article_ArticleId, Article_Name, Article_UnitPricePerWeek, Article_Owner_OwnerId, Article_Owner_Name
FROM         dbo.Dm_Shop_CartItem");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}