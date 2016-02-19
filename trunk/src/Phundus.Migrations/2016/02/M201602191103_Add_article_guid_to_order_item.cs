namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602191103)]
    public class M201602191103_Add_article_guid_to_order_item : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Shop_OrderItem").AddColumn("ArticleGuid").AsGuid().Nullable();

            Execute.Sql(@"UPDATE [Dm_Shop_OrderItem]
SET [ArticleGuid] = (SELECT [ArticleGuid] FROM [Dm_Inventory_Article]
WHERE [Dm_Inventory_Article].[Id] = [Dm_Shop_OrderItem].[ArticleId])");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    [Migration(201602191109)]
    public class M201602191109_Set_ArticleGuid_as_not_nullable : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"UPDATE [Dm_Shop_OrderItem] SET [ArticleGuid] = CONVERT(uniqueidentifier, '00000000-0000-0000-0000-000000000000') WHERE [ArticleGuid] IS NULL");

            Alter.Column("ArticleGuid").OnTable("Dm_Shop_OrderItem").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}