namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201302252312)]
    public class M201302252312_AddUniqueKeyToOrganizationMembership : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(
                "ALTER TABLE [OrganizationMembership] ADD CONSTRAINT IX_UserId_OrganizationId UNIQUE([UserId], [OrganizationId])");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE [OrganizationMembership] DROP CONSTRAINT IX_UserId_OrganizationId");
        }
    }
}