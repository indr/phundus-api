using FluentMigrator;

namespace phiNdus.fundus.DbMigrations
{
    [Migration(201204211657)]
    public class M0006_CreateTableFieldDefinition : MigrationBase
    {
        private const string TableName = "FieldDefinition";

        public override void Up()
        {
            Create.Table(TableName).InSchema(SchemaName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("DataType").AsByte().NotNullable()
                .WithColumn("IsSystem").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsDefault").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsColumn").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsAttachable").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("Position").AsInt32().NotNullable().WithDefaultValue(255);
        }

        public override void Down()
        {
            Delete.Table(TableName).InSchema(SchemaName);
        }
    }
}