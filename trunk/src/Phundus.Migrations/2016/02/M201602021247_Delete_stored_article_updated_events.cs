namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602021247)]
    public class M201602021247_Delete_stored_article_updated_events : MigrationBase
    {
        public override void Up()
        {
            Delete.FromTable("StoredEvents")
                .Row(new {TypeName = "Phundus.Inventory.Articles.Model.ArticleUpdated, Phundus.Core"});
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}