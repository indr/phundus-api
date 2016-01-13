namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201601130718)]
    public class M201601130718AddPositionDaysAndItemTotalToCartItem : MigrationBase
    {
        public override void Up()
        {
            // ¯\_(ツ)_/¯
            Delete.FromTable("Dm_Shop_CartItem").AllRows();

            Alter.Table("Dm_Shop_CartItem")
                .AddColumn("Position").AsInt32().NotNullable()
                .AddColumn("Days").AsInt32().NotNullable()
                .AddColumn("ItemTotal").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}