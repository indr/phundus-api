using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201204
{
    [Migration(201205042139)]
    public class M0015_AddColumnJsNumber : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("User").InSchema(SchemaName).AddColumn("JsNumber").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("JsNumber").FromTable("User").InSchema(SchemaName);
        }
    }
}