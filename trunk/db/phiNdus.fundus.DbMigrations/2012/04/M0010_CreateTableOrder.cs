using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211716)]
    public class M0010_CreateTableOrder : MigrationBase
    {
        private const string TableName = "Order";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("CreateDate").AsDateTime().NotNullable()
                .WithColumn("Status").AsByte().NotNullable()
                .WithColumn("ReserverId").AsInt32().NotNullable()
                .WithColumn("ModifyDate").AsDateTime().Nullable()
                .WithColumn("ModifierId").AsInt32().Nullable();

            Create.ForeignKey("FkOrderToReserver")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("ReserverId")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create.ForeignKey("FkOrderToModifier")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("ModifierId")
                .ToTable("User").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Execute.Sql(@"dbcc checkident('Order', reseed, 10000)");
        }

        public override void Down()
        {
            Delete.ForeignKey("FkOrderToModifier").OnTable(TableName).InSchema(SchemaName);

            Delete.ForeignKey("FkOrderToReserver").OnTable(TableName).InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}