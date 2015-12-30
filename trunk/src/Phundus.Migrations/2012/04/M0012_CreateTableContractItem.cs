namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201204211720)]
    public class M0012_CreateTableContractItem : MigrationBase
    {
        private const string TableName = "ContractItem";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("ContractId").AsInt32().NotNullable()
                .WithColumn("OrderItemId").AsInt32().NotNullable()
                .WithColumn("ArticleId").AsInt32().NotNullable()
                .WithColumn("ReturnDate").AsDateTime().Nullable()
                .WithColumn("InventoryCode").AsString(255).Nullable()
                .WithColumn("Amount").AsInt32().NotNullable();

            Create.ForeignKey("FkContractItemToContract")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("ContractId")
                .ToTable("Contract").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey("FkContractItemToOrderItem")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrderItemId")
                .ToTable("OrderItem").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey("FkContractItemToArticle")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("ArticleId")
                .ToTable("Article").InSchema(SchemaName)
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkContractItemToArticle").OnTable(TableName).InSchema(SchemaName);

            Delete.ForeignKey("FkContractItemToOrderItem").OnTable(TableName).InSchema(SchemaName);

            Delete.ForeignKey("FkContractItemToContract").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}