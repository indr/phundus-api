namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201601212344)]
    public class M201601212344_Add_ArticleGuid_To_Dm_Inventory_Article : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Dm_Inventory_Article")
                .AddColumn("ArticleGuid")
                .AsGuid()
                .NotNullable()
                .WithDefault(SystemMethods.NewGuid);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}