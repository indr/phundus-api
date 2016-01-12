namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601122252)]
    public class M201601122252AddIdAndGuidToCartAndCartItem : MigrationBase
    {
        public override void Up()
        {
            // ¯\_(ツ)_/¯
            Delete.FromTable("Dm_Shop_CartItem").AllRows();
            Delete.FromTable("Dm_Shop_Cart").AllRows();

            Rename.Column("Id").OnTable("Dm_Shop_Cart").To("CartId");
            Create.Column("CartGuid").OnTable("Dm_Shop_Cart").AsGuid().NotNullable();
            Rename.Column("CustomerId").OnTable("Dm_Shop_Cart").To("UserId");
            Create.Column("UserGuid").OnTable("Dm_Shop_Cart").AsGuid().NotNullable();

            Rename.Column("CartItemId").OnTable("Dm_Shop_CartItem").To("CartItemGuid");
            Rename.Column("Id").OnTable("Dm_Shop_CartItem").To("CartItemId");
            Create.Column("CartGuid").OnTable("Dm_Shop_CartItem").AsGuid().NotNullable();

            Rename.Column("From").OnTable("Dm_Shop_CartItem").To("FromUtc");
            Rename.Column("To").OnTable("Dm_Shop_CartItem").To("ToUtc");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}