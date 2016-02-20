namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602200932)]
    public class M201602200932_Add_article_guid_to_cart_item : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [Dm_Shop_CartItem]");

            Alter.Table("Dm_Shop_CartItem").AddColumn("Article_ArticleGuid").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}