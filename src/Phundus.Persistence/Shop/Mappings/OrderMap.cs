namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.CreateDate, "CreateDate");
            Map(x => x.Status, "Status").CustomType<OrderStatus>();
            Map(x => x.OrganizationId, "OrganizationId");

            References(x => x.Reserver, "ReserverId");

            HasMany(x => x.Items).AsSet()
                .KeyColumn("OrderId").Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }
}