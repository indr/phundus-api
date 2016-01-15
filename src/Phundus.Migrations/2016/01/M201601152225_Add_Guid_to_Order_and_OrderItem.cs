namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601152225)]
    public class M201601152225_Add_Guid_to_Order_and_OrderItem : MigrationBase
    {
        public override void Up()
        {
            Create.Column("OrderGuid").OnTable("Dm_Shop_Order").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid);
            Create.Column("OrderItemGuid").OnTable("Dm_Shop_OrderItem").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}