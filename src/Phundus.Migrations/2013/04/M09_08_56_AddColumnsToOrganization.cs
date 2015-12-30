namespace Phundus.Migrations
{
    [Dated(0, 2013, 04, 09, 08, 56)]
    public class M09_08_56_AddColumnsToOrganization : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Organization").InSchema(SchemaName)
                 .AddColumn("EmailAddress").AsString(255).Nullable()
                 .AddColumn("Website").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Website").FromTable("Organization").InSchema(SchemaName);
            Delete.Column("EmailAddress").FromTable("Organization").InSchema(SchemaName);
        }
    }
}