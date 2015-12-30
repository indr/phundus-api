namespace Phundus.Migrations
{
    using System.Data;

    [Dated(0, 2013, 4, 1, 11, 13)]
    public class M01_11_13_AlterFkOrderItemToOrder : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("FkOrderItemToOrder").OnTable("OrderItem").InSchema(SchemaName);

            Create.ForeignKey("Fk_OrderItemToOrder")
                .FromTable("OrderItem").InSchema(SchemaName)
                .ForeignColumn("OrderId")
                .ToTable("Order").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            // Nothing to do here
        }
    }
}