using System.Data;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201302
{
    [Migration(201303211953)]
    public class M201303211953_ArticleAddColumnOrganizationId : MigrationBase
    {
        private const string TableName = "Article";

        public override void Up()
        {
            Alter.Table(TableName).InSchema(SchemaName).AddColumn("OrganizationId").AsInt32().Nullable();

            Execute.Sql("update [Article] set [OrganizationId] = 1001");

            Alter.Table(TableName).InSchema(SchemaName).AlterColumn("OrganizationId").AsInt32().NotNullable();

            Create.ForeignKey("Fk_ArticleToOrganization")
                .FromTable(TableName).InSchema(SchemaName)
                .ForeignColumn("OrganizationId")
                .ToTable("Organization").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.None);
        }

        public override void Down()
        {
            Delete.ForeignKey("Fk_ArticleToOrganization");
            Delete.Column("OrganizationId").FromTable(TableName).InSchema(SchemaName);
        }
    }
}