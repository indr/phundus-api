namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Projections;

    public class OrderDataMap : ClassMap<OrderData>
    {
        public OrderDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Orders");

            Id(x => x.OrderId).GeneratedBy.Assigned();
            Map(x => x.OrderShortId);
            Map(x => x.CreatedAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.ModifiedAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.Status).CustomType<OrderData.OrderStatus>();
            Map(x => x.OrderTotal);

            Map(x => x.LessorId);
            Map(x => x.LessorName);
            Map(x => x.LessorStreet);
            Map(x => x.LessorPostcode);
            Map(x => x.LessorCity);
            Map(x => x.LessorEmailAddress);
            Map(x => x.LessorPhoneNumber);

            Map(x => x.LesseeId);
            Map(x => x.LesseeFirstName);
            Map(x => x.LesseeLastName);
            Map(x => x.LesseeStreet);
            Map(x => x.LesseePostcode);
            Map(x => x.LesseeCity);
            Map(x => x.LesseeEmailAddress);
            Map(x => x.LesseePhoneNumber);

            HasMany(x => x.Items).KeyColumn("OrderId").Inverse().Cascade.AllDeleteOrphan();
        }
    }

    public class OrderLineDataMap : ClassMap<OrderLineData>
    {
        public OrderLineDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Orders_Lines");

            Id(x => x.Id).GeneratedBy.GuidComb();

            Map(x => x.LineId);
            References(x => x.Order, "OrderId");

            Map(x => x.ArticleId);
            Map(x => x.ArticleShortId);

            Map(x => x.Text);
            Map(x => x.FromUtc).CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc).CustomType<UtcDateTimeType>();
            Map(x => x.Quantity);
            Map(x => x.UnitPricePerWeek);
            Map(x => x.LineTotal);
        }
    }
}