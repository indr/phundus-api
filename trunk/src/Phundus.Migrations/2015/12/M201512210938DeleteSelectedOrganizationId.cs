namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201512210938)]
    public class M201512210938DeleteSelectedOrganizationId : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("Fk_UserToOrganization").OnTable("User");
            Delete.Column("SelectedOrganizationId").FromTable("User").InSchema(SchemaName);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}