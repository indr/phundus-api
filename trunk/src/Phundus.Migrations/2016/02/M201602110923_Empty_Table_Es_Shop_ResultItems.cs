namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602110923)]
    public class M201602110923_Empty_Table_Es_Shop_ResultItems : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Shop_ResultItems").Exists())
                Delete.FromTable("Es_Shop_ResultItems").AllRows();
            EmptyTableAndResetTracker("Es_Shop_ResultItems", @"Phundus.Shop.Projections.ResultItemsProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}