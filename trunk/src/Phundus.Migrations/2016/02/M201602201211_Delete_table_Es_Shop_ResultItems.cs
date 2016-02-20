namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602201211)]
    public class M201602201211_Delete_table_Es_Shop_ResultItems : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Es_Shop_ResultItems");
            ResetProcessedNotififactionTracker("Phundus.Shop.Projections.ResultItemsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}