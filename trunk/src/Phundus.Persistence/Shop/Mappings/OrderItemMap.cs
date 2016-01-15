namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Orders.Model;

    public class OrderItemMap : ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_OrderItem");
            Id(x => x.Id, "OrderItemGuid").GeneratedBy.Assigned();
            Version(x => x.Version);

            References(x => x.Order, "OrderId");

            Map(x => x.ArticleId, "ArticleId");
            Map(x => x.Text, "Text");
            Map(x => x.UnitPrice, "UnitPrice");

            Map(x => x.FromUtc, "[FromUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc, "[ToUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.Amount);

            Map(x => x.ItemTotal, "[Total]");
        }
    }
}