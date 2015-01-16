namespace phiNdus.fundus.DbMigrations
{
    using System;
    using FluentMigrator;

    [Migration(201501160956)]
    public class M201501160956AddColumnStockIdToArticle : MigrationBase
    {
        public override void Up()
        {
            Alter.Table("Article").AddColumn("StockId").AsString().Nullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}