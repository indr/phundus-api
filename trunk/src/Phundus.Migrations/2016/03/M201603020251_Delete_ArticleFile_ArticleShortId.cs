namespace Phundus.Migrations
{
    using FluentMigrator;
    using FluentMigrator.Runner.Extensions;

    [Migration(201603020251)]
    public class M201603020251_Delete_ArticleFile_ArticleShortId : MigrationBase
    {
        public override void Up()
        {
            Delete.Column("ArticleShortId").FromTable("Dm_Inventory_ArticleFile");
        }
    }
}