namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601160535)]
    public class M201601160535_Recreate_View_Shop_Carts : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_CartItems];");
            Execute.Sql(@"DROP VIEW [dbo].[View_Shop_Carts]");

            Execute.Sql(@"
CREATE VIEW [dbo].[View_Shop_Carts]
AS
SELECT     CartGuid, UserGuid
FROM         dbo.Dm_Shop_Cart;");


            Execute.Sql(@"
CREATE VIEW [dbo].[View_Shop_CartItems]
AS
SELECT     CartItemGuid, CartGuid, Position, FromUtc, ToUtc, Days, Quantity, ItemTotal, Article_ArticleId, Article_Name, Article_UnitPricePerWeek, Article_Owner_OwnerId, 
                      Article_Owner_Name
FROM         dbo.Dm_Shop_CartItem");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(201601160510)]
    public class M201601160510_Delete_UserId_and_CartId_from_Dm_Shop_Cart : MigrationBase
    {
        public override void Up()
        {
            Delete_CartItem_CartItemId();
            Delete_CartItem_CartId();
            
            Delete_Cart_CartId();
            Delete_Cart_UserId();
        }

        private void Delete_CartItem_CartId()
        {
            const string tableName = "Dm_Shop_CartItem";

            Delete.ForeignKey("FK_CartItemToCart").OnTable(tableName);
            Delete.Column("CartId").FromTable(tableName);
        }

        private void Delete_CartItem_CartItemId()
        {
            const string tableName = "Dm_Shop_CartItem";

            Delete.PrimaryKey("PK_CartItem").FromTable(tableName);
            Delete.Column("CartItemId").FromTable(tableName);
        }

        private void Delete_Cart_CartId()
        {
            const string tableName = "Dm_Shop_Cart";

            Delete.PrimaryKey("PK_Cart").FromTable(tableName);
            Delete.Column("CartId").FromTable(tableName);
        }

        private void Delete_Cart_UserId()
        {
            const string tableName = "Dm_Shop_Cart";

            Delete.ForeignKey("FkCartToUser").OnTable(tableName);
            Delete.Column("UserId").FromTable(tableName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}