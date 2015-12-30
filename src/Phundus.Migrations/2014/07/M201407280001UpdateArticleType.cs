namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201407280001)]
    public class M201407280001UpdateArticleType : MigrationBase
    {
        public override void Up()
        {
            Update.Table("Article").Set(new { @Type = "Phundus.Core.InventoryCtx.Model.Article"}).AllRows();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}