namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602080812)]
    public class M201602080812_Drop_Table_Es_Inventory_Articles : MigrationBase
    {
        public override void Up()
        {
            Delete.Table("Es_Inventory_Articles");
            DeleteTracker(@"Phundus.Inventory.Projections.ArticlesProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}