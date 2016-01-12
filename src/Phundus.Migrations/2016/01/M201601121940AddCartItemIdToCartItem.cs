namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601121940)]
    public class M201601121940AddCartItemIdToCartItem : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Shop_CartItem").AddColumn("CartItemId")
                .AsGuid().WithDefault(SystemMethods.NewGuid).NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}