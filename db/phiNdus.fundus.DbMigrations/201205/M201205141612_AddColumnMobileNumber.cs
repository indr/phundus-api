using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201204
{
    [Migration(201205131612)]
    public class M201205131612_AddColumnMobileNumber : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("User").InSchema(SchemaName).AddColumn("MobileNumber").AsString(30).Nullable();
        }

        public override void Down()
        {
            Delete.Column("MobileNumber").FromTable("User").InSchema(SchemaName);
        }
    }
}