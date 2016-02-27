namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using NHibernate.Type;
    using Phundus.Shop.Orders.Model;

    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_Order");

            Id(x => x.Id, "OrderShortId").GeneratedBy.Assigned();            
            Version(x => x.Version);
            Component(x => x.OrderId, a => a.Map(x => x.Id, "OrderGuid"));

            Map(x => x.CreatedAtUtc, "CreatedUtc").CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").CustomType<OrderStatus>();

            Component(x => x.Lessor, c =>
            {
                c.Component(x => x.LessorId, c2 =>
                    c2.Map(x => x.Id, "Lessor_LessorId"));
                c.Map(x => x.Name, "Lessor_Name");
            });

            Component(x => x.Lessee, c =>
            {
                c.Component(x => x.LesseeId, c2 =>
                    c2.Map(x => x.Id, "Lessee_LesseeGuid"));
                c.Map(x => x.FirstName, "Borrower_FirstName");
                c.Map(x => x.LastName, "Borrower_LastName");
                c.Map(x => x.Street, "Borrower_Street");
                c.Map(x => x.Postcode, "Borrower_Postcode");
                c.Map(x => x.City, "Borrower_City");
                c.Map(x => x.EmailAddress, "Borrower_EmailAddress");
                c.Map(x => x.PhoneNumber, "Borrower_MobilePhoneNumber");
                c.Map(x => x.MemberNumber, "Borrower_MemberNumber");
            });

            HasMany(x => x.Items).AsSet()
                .KeyColumn("OrderId").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }

    public class OrderItemMap : ClassMap<OrderItem>
    {
        public OrderItemMap()
        {
            SchemaAction.Validate();

            Table("Dm_Shop_OrderItem");
            Id(x => x.Id, "OrderItemGuid").GeneratedBy.Assigned();
            Version(x => x.Version);

            References(x => x.Order, "OrderId");

            Component(x => x.ArticleId, a =>
                a.Map(x => x.Id, "ArticleGuid"));
            Component(x => x.ArticleShortId, a =>
                a.Map(x => x.Id, "ArticleId"));
            Map(x => x.Text, "Text");
            Map(x => x.UnitPrice, "UnitPrice");

            Map(x => x.FromUtc, "[FromUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.ToUtc, "[ToUtc]").CustomType<UtcDateTimeType>();
            Map(x => x.Amount);

            Map(x => x.ItemTotal, "[Total]");
        }
    }
}