namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class OrderItemMap : ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_OrderItem");
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            References(x => x.Order, "OrderId");

            Map(x => x.ArticleId, "ArticleId");
            Map(x => x.Text, "Text");
            Map(x => x.UnitPrice, "UnitPrice");

            Map(x => x.FromUtc, "[FromUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc, "[ToUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.Amount);

            Map(x => x.LineTotal, "[Total]");
        }
    }
}