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
}