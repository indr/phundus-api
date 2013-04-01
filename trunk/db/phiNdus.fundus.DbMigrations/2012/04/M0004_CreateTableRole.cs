using System.Data;
using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211650)]
    public class M0004_CreateTableRole : MigrationBase
    {
        private const string TableName = "Role";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).Nullable();

            Create.ForeignKey("FkUserToRole")
                .FromTable("User").InSchema(SchemaName)
                .ForeignColumn("RoleId")
                .ToTable("Role").InSchema(SchemaName)
                .PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.SetNull);
        }

        public override void Down()
        {
            Delete.ForeignKey("FkUserToRole").OnTable("User").InSchema(SchemaName);

            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}