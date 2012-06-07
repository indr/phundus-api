using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201206072135)]
    public class M201206072135_CreateTableCart : MigrationBase
    {
        private const string TableName = "Cart";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("CustomerId").AsInt32().NotNullable();

            Create.ForeignKey("FkCartToUser")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("CustomerId")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Execute.Sql(@"dbcc checkident('Cart', reseed, 10000)");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkCartToUser").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}