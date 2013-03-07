using System.Data;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201302
{
    [Migration(201303080004)]
    public class M201303080004_AddColumnSelectedOrganizationId : MigrationBase
    {
        private const string TableName = "User";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName).AddColumn("SelectedOrganizationId").AsInt32().Nullable();

            Create.ForeignKey("Fk_UserToOrganization")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("SelectedOrganizationId")
                .ToTable("Organization").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.None);
        }

        public override void Down()
        {
            Delete.ForeignKey("Fk_UserToOrganization");
            Delete.Column("SelectedOrganizationId").FromTable(TableName).InSchema(SchemaName);
        }
    }
}