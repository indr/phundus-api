namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class OrderItemsProjectionRowMap : ClassMap<OrderItemsProjectionRow>
    {
        public OrderItemsProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_OrderItems");

            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.OrderId);
        }
    }
}