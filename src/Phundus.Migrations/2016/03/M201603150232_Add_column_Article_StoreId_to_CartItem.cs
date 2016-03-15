namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603150232)]
    public class M201603150232_Add_column_Article_StoreId_to_CartItem : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_CartItems]");

            Delete.FromTable("Dm_Shop_CartItem").AllRows();
            Alter.Table("Dm_Shop_CartItem").AddColumn("Article_StoreId").AsGuid().NotNullable();

            Execute.Sql(@"CREATE VIEW [dbo].[View_Shop_CartItems]
AS
SELECT CartItemGuid, CartGuid, Position, FromUtc, ToUtc, Days, Quantity, ItemTotal, Article_ArticleId as ArticleShortId, ArticleId, Article_StoreId as StoreId, Article_Name as Text, Article_UnitPricePerWeek as UnitPricePerWeek, Article_Owner_OwnerId as LessorId, Article_Owner_Name as LessorName
FROM         dbo.Dm_Shop_CartItem
LEFT JOIN dbo.Dm_Inventory_Article ON Dm_Shop_CartItem.Article_ArticleGuid = dbo.Dm_Inventory_Article.ArticleId");
        }
    }
}