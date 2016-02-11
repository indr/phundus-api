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

            Id(x => x.Id, "Id").GeneratedBy.Native();
            //CompositeId(x => x.OrderShortId).KeyProperty(x => x.Id, "Id");
            Version(x => x.Version);
            Component(x => x.OrderId, a => a.Map(x => x.Id, "OrderGuid"));

            Map(x => x.CreatedUtc, "CreatedUtc").CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").CustomType<OrderStatus>();
            Map(x => x.ModifiedUtc, "ModifiedUtc").CustomType<UtcDateTimeType>();

            Component(x => x.Lessor, c =>
            {
                c.Map(x => x.LessorGuid, "Lessor_LessorId");
                c.Map(x => x.Name, "Lessor_Name");
            });

            Component(x => x.Lessee, c =>
            {
                c.Map(x => x.LesseeGuid, "Lessee_LesseeGuid");
                c.Map(x => x.FirstName, "Borrower_FirstName");
                c.Map(x => x.LastName, "Borrower_LastName");
                c.Map(x => x.Street, "Borrower_Street");
                c.Map(x => x.Postcode, "Borrower_Postcode");
                c.Map(x => x.City, "Borrower_City");
                c.Map(x => x.EmailAddress, "Borrower_EmailAddress");
                c.Map(x => x.MobilePhoneNumber, "Borrower_MobilePhoneNumber");
                c.Map(x => x.MemberNumber, "Borrower_MemberNumber");
            });

            HasMany(x => x.Items).AsSet()
                .KeyColumn("OrderId").Inverse()
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan();
        }
    }
}