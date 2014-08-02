namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201408021809)]
    public class M201408021809AddColumnsToArticle : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Article")
                .AddColumn("Name").AsString().Nullable()
                .AddColumn("Price").AsDecimal().Nullable()
                .AddColumn("Description").AsString().Nullable()
                .AddColumn("Data").AsString().Nullable()
                .AddColumn("Stock").AsInt32().Nullable()
                .AddColumn("Color").AsString().Nullable()
                .AddColumn("Price_InfoCard").AsDecimal().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}