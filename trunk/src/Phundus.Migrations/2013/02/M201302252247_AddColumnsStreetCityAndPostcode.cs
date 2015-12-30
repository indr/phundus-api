namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201302252247)]
    public class M201302252247_AddColumnsStreetCityAndPostcode : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("User").InSchema(SchemaName).AddColumn("Street").AsString(30).Nullable();
            Alter.Table("User").InSchema(SchemaName).AddColumn("Postcode").AsString(10).Nullable();
            Alter.Table("User").InSchema(SchemaName).AddColumn("City").AsString(30).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Street").FromTable("User").InSchema(SchemaName);
            Delete.Column("Postcode").FromTable("User").InSchema(SchemaName);
            Delete.Column("City").FromTable("User").InSchema(SchemaName);
        }
    }
}