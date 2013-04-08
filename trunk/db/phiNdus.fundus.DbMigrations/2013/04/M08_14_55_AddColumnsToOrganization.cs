namespace phiNdus.fundus.DbMigrations
{
    using System;

    [Dated(0, 2013, 4, 8, 14, 55)]
    public class M08_14_55_AddColumnsToOrganization : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Organization").InSchema(SchemaName)
                 .AddColumn("Address").AsString(255).Nullable()
                 .AddColumn("Coordinate").AsString(50).Nullable()
                 .AddColumn("Startpage").AsString(Int32.MaxValue).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Address").FromTable("Organization").InSchema(SchemaName);
            Delete.Column("Coordinate").FromTable("Organization").InSchema(SchemaName);
            Delete.Column("Startpage").FromTable("Organization").InSchema(SchemaName);
        }
    }
}