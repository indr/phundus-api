namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201603020128)]
    public class M201603020128_Reset_Shop_Orders_projection : MigrationBase
    {
        public override void Up()
        {
            DeleteTable("Es_Shop_Orders_Lines");
            DeleteTable("Es_Shop_Orders");
            DeleteTracker("OrderProjection");
        }
    }
}