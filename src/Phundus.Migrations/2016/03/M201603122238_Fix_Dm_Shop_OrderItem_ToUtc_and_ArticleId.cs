namespace Phundus.Migrations
{
    using System;
    using System.Collections.Generic;
    using FluentMigrator;

    [Migration(201603122238)]
    public class M201603122238_Fix_Dm_Shop_OrderItem_ToUtc_and_ArticleId : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE [Dm_Shop_OrderItem] SET [ToUtc] = [FromUtc] WHERE [ToUtc] < [FromUtc]");
            foreach (var each in DeletedArticleIdMap)
                UpdateArticleId(each);
        }

        private void UpdateArticleId(KeyValuePair<int, Guid> each)
        {
            Execute.Sql(@"UPDATE [Dm_Shop_OrderItem] SET [ArticleGuid] = '" + each.Value + "' WHERE [ArticleId] = " +
                        each.Key);
        }
    }
}