namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201204211717)]
    public class M0011_CreateTableOrderItem : MigrationBase
    {
        private const string TableName = "OrderItem";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("OrderId").AsInt32().NotNullable()
                .WithColumn("Amount").AsInt32().NotNullable()
                .WithColumn("From").AsDateTime().NotNullable()
                .WithColumn("To").AsDateTime().NotNullable()
                .WithColumn("ArticleId").AsInt32().NotNullable();

            Create.ForeignKey("FkOrderItemToOrder")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrderId")
                .ToTable("Order").InSchema(SchemaName)
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkOrderItemToOrder").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}