namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601170740)]
    public class M201601170740_Remove_Path_From_Article_Images : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE [Dm_Inventory_ArticleFile] SET [FileName] = SUBSTRING([FileName], LEN([FileName]) - CHARINDEX('\', REVERSE([FileName])) + 2, 1024)");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}