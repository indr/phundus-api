using FluentMigrator;

namespace phiNdus.fundus.DbMigrations._201204
{
    [Migration(201302171552)]
    public class M201302171552_AddColumnRequestedEmail : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Membership").InSchema(SchemaName).AddColumn("RequestedEmail").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("RequestedEmail").FromTable("Membership").InSchema(SchemaName);
        }
    }
}