namespace phiNdus.fundus.DbMigrations
{
    using System.Data;
    using FluentMigrator;

    [Migration(201303212259)]
    public class M201303212259_OrderAddColumnOrganizationId : MigrationBase
    {
        private const string TableName = "Order";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName).AddColumn("OrganizationId").AsInt32().Nullable();

            Execute.Sql("update [Order] set [OrganizationId] = 1001");

            Alter.Table(TableName).InSchema(SchemaName).AlterColumn("OrganizationId").AsInt32().NotNullable();

            Create.ForeignKey("Fk_OrderToOrganization")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrganizationId")
                .ToTable("Organization").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.None);
        }

        public override void Down()
        {
            Delete.ForeignKey("Fk_OrderToOrganization");
            Delete.Column("OrganizationId").FromTable(TableName).InSchema(SchemaName);
        }
    }
}