namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Orders.Model;
    using FluentNHibernate.Mapping;

    public class OrderItemMap : ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            Table("OrderItem");
            Id(x => x.Id).GeneratedBy.Assigned();
            Version(x => x.Version);

            References(x => x.Order, "OrderId");

            Map(x => x.Amount);
            Map(x => x.From, "[From]");
            Map(x => x.To, "[To]");

            References(x => x.Article, "ArticleId");
        }
    }
}