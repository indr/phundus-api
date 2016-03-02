namespace Phundus.Migrations
{
    using System.Data;
    using FluentMigrator;

    [Migration(201603020229)]
    public class M201603020229_Create_primary_and_foreign_keys : MigrationBase
    {
        public override void Up()
        {
            Rename.Column("ArticleGuid").OnTable("Dm_Inventory_Article").To("ArticleId");
            Create.PrimaryKey("Pk_Article").OnTable("Dm_Inventory_Article").Column("ArticleId");

            Create.ForeignKey("Fk_ArticleFileToArticle")
                .FromTable("Dm_Inventory_ArticleFile").ForeignColumn("ArticleId")
                .ToTable("Dm_Inventory_Article").PrimaryColumn("ArticleId")
                .OnDeleteOrUpdate(Rule.Cascade);
        }
    }
}