namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;


    [Migration(201512301153)]
    public class M201512301153AddStoreToArticle : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Article").InSchema(SchemaName).AddColumn("StoreId").AsGuid().Nullable();

            Execute.Sql(@"UPDATE [Article] SET [Article].[StoreId] = (SELECT s.[StoreId] FROM [Dm_Store] s WHERE s.[Owner_OwnerId] = [Article].[Owner_OwnerId])");

            Alter.Table("Article").InSchema(SchemaName).AlterColumn("StoreId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}