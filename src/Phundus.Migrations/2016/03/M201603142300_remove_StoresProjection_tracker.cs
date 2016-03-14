namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603142300)]
    public class M201603142300_remove_StoresProjection_tracker : MigrationBase
    {
        public override void Up()
        {
            DeleteTableAndTracker("Es_Inventory_Stores", "StoresProjection");
        }
    }
}