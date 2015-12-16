namespace phiNdus.fundus.DbMigrations
{
    using System;
    using System.Data;
    using FluentMigrator;

    [Migration(201512160809)]
    public class M201512160809CreateForeignKeyCartItemToArticle : MigrationBase
    {
        public override void Up()
        {
            Execute.Sql(@"DELETE FROM [CartItem] WHERE [ArticleId] NOT IN (SELECT [Id] FROM [Article])");

            Create.ForeignKey().FromTable("CartItem").ForeignColumn("ArticleId").ToTable("Article").PrimaryColumn("Id").OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}