namespace Phundus.Migrations
{
    using FluentMigrator;

    [Migration(201602291407)]
    public class M201602291407_Reinsert_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            Reinsert("Phundus.Inventory.Articles.Model.PricesChanged, Phundus.Core");
            Reinsert("Phundus.Inventory.Articles.Model.ArticleDeleted, Phundus.Core");

            Reinsert("Phundus.Shop.Orders.Model.OrderRejected, Phundus.Core");
            Reinsert("Phundus.Shop.Orders.Model.OrderApproved, Phundus.Core");
            Reinsert("Phundus.Shop.Orders.Model.OrderClosed, Phundus.Core");
        }

        public override void Up()
        {
            base.Up();

            EmptyTableAndResetTracker("Es_Dashboard_EventLog", "EventLogProjection");
        }
    }
}