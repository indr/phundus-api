namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603020225)]
    public class M201603020225_Alter_Table_Dm_Inventory_Article : MigrationBase
    {
        public override void Up()
        {
            Delete.ForeignKey("FkImageToArticle").OnTable("Dm_Inventory_ArticleFile");
            Delete.PrimaryKey("PK_Article").FromTable("Dm_Inventory_Article");
            
            Alter.Table("Dm_Inventory_Article").AddColumn("ArticleShortId").AsInt32().Nullable();
            Execute.Sql(@"UPDATE [Dm_Inventory_Article] SET [ArticleShortId] = [Id]");
            Delete.Column("Id").FromTable("Dm_Inventory_Article");
            Alter.Column("ArticleShortId").OnTable("Dm_Inventory_Article").AsInt32().NotNullable();

            Rename.Column("ArticleId").OnTable("Dm_Inventory_ArticleFile").To("ArticleShortId");
            Alter.Table("Dm_Inventory_ArticleFile").AddColumn("ArticleId").AsGuid().Nullable();
            Execute.Sql(@"UPDATE [Dm_Inventory_ArticleFile] SET [ArticleId] = (SELECT [ArticleGuid] FROM [Dm_Inventory_Article] WHERE [Dm_Inventory_Article].[ArticleShortId] = [Dm_Inventory_ArticleFile].[ArticleShortId])");
            Alter.Column("ArticleId").OnTable("Dm_Inventory_ArticleFile").AsGuid().NotNullable();
        }
    }
}