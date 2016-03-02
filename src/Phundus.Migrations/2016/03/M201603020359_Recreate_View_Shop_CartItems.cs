namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603020359)]    
    public class M201603020359_Recreate_View_Shop_CartItems : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_CartItems]");
            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_CartItems]
AS
SELECT CartItemGuid, CartGuid, Position, FromUtc, ToUtc, Days, Quantity, ItemTotal, Article_ArticleId as ArticleShortId, ArticleId, Article_Name as Text, Article_UnitPricePerWeek as UnitPricePerWeek, Article_Owner_OwnerId as LessorId, Article_Owner_Name as LessorName
FROM         dbo.Dm_Shop_CartItem
LEFT JOIN dbo.Dm_Inventory_Article ON Dm_Shop_CartItem.Article_ArticleGuid = dbo.Dm_Inventory_Article.ArticleId");
        }        
    }
}