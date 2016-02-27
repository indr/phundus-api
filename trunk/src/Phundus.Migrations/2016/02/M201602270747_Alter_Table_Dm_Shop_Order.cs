namespace Phundus.Migrations
{
    using System;
    using System.Data;
    using FluentMigrator;

    [Migration(201602270759)]
    public class M201602270759_Create_Primary_and_Foreign_Key : MigrationBase
    {
        public override void Up()
        {
            Create.PrimaryKey("PK_Order").OnTable("Dm_Shop_Order").Column("OrderShortId");

            Create.ForeignKey("Fk_OrderItemToOrder").FromTable("Dm_Shop_OrderItem").ForeignColumn("OrderId").ToTable("Dm_Shop_Order").PrimaryColumn("OrderShortId").OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(201602270748)]
    public class M201602270748_Alter_Table_Dm_Shop_Order : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("Fk_OrderItemToOrder").OnTable("Dm_Shop_OrderItem");
            Delete.PrimaryKey("PK_Order").FromTable("Dm_Shop_Order");
            Alter.Table("Dm_Shop_Order").AddColumn("OrderShortId").AsInt32().Nullable();

            Execute.Sql(@"UPDATE [Dm_Shop_Order] SET [OrderShortId] = [Id]");

            Delete.Column("Id").FromTable("Dm_Shop_Order");

            Alter.Column("OrderShortId").OnTable("Dm_Shop_Order").AsInt32().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}