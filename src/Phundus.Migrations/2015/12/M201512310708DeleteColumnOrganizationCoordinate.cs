namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512310708)]
    public class M201512310708DeleteColumnOrganizationCoordinate : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("Coordinate").FromTable("Organization").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}