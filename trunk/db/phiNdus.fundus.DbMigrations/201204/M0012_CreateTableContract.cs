using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211719)]
    public class M0012_CreateTableContract : MigrationBase
    {
        private const string TableName = "Contract";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable()
                .WithColumn("OrderId").AsInt32().NotNullable()
                .WithColumn("BorrowerId").AsInt32().NotNullable()
                .WithColumn("From").AsDateTime().NotNullable()
                .WithColumn("To").AsDateTime().NotNullable();

            Create.ForeignKey("FkContractToOrder")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrderId")
                .ToTable("Order").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey("FkContractToBorrower")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("BorrowerId")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkContractToBorrower").OnTable(TableName).InSchema(SchemaName);

            Delete.ForeignKey("FkContractToOrder").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}