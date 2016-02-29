namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602290608)]
    public class M201602290608_Delete_StoredEvents_OrganizationUpdated : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [StoredEvents] WHERE [TypeName] LIKE '%OrganizationUpdated, %'");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}