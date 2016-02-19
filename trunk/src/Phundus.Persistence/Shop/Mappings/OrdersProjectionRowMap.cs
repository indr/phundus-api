namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class OrdersProjectionRowMap : ClassMap<OrdersProjectionRow>
    {
        public OrdersProjectionRowMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Orders");

            Id(x => x.OrderId).GeneratedBy.Assigned();
            Map(x => x.OrderShortId);
            Map(x => x.CreatedAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.Status).CustomType<OrdersProjectionRow.OrderStatus>();

            Map(x => x.LessorId);
            Map(x => x.LesseeId);

            HasMany(x => x.Items).KeyColumn("OrderId").ReadOnly().Inverse().ForeignKeyCascadeOnDelete();
        }
    }
}