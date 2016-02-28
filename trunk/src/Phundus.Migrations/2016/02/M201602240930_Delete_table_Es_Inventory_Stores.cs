namespace Phundus.Migrations
{
    using System;
    using FluentMigrator;

    [Migration(201602240930)]
    public class M201602240930_Delete_table_Es_Inventory_Stores : MigrationBase
    {
        public override void Up()
        {
            if (Schema.Table("Es_Inventory_Stores").Exists())
                Delete.Table("Es_Inventory_Stores");
            ResetTracker("Phundus.Inventory.Projections.StoresProjection");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}