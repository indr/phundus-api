namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602030710)]
    public class M201602030710_Delete_empty_article_stored_events : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [StoredEvents] WHERE [Serialization] = 0 AND [TypeName] LIKE '%Article%'");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}