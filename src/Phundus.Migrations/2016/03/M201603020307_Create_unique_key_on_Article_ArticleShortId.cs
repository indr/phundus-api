namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603020307)]
    public class M201603020307_Create_unique_key_on_Article_ArticleShortId : MigrationBase
    {
        public override void Up()
        {
            Create.UniqueConstraint("UK_ArticleShortId").OnTable("Dm_Inventory_Article").Column("ArticleShortId");

            InsertSequence("ArticleShortId", "SELECT MAX(ArticleShortId) FROM [Dm_Inventory_Article]");
        }
    }
}