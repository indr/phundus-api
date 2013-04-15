namespace phiNdus.fundus.DbMigrations
{
    [Dated(0, 2013, 04, 15, 15, 42)]
    public class M15_15_42 : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Organization").InSchema(SchemaName)
                 .AddColumn("DocTemplateFileName").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DocTemplateFileName").FromTable("Organization").InSchema(SchemaName);
        }
    }
}