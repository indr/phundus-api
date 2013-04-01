using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201206072146)]
    public class M201206072146_CreateTableCartItem : MigrationBase
    {
        private const string TableName = "CartItem";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey() // HiLo
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("CartId").AsInt32().NotNullable()
                .WithColumn("Quantity").AsInt32().NotNullable()
                .WithColumn("From").AsDateTime().NotNullable()
                .WithColumn("To").AsDateTime().NotNullable()
                .WithColumn("ArticleId").AsInt32().NotNullable();

            Create.ForeignKey("FkCartItemToCart")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("CartId")
                .ToTable("Cart").InSchema(SchemaName)
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkCartItemToCart").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}