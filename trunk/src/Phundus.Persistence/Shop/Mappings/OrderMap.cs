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
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.CreatedUtc, "CreatedUtc").CustomType<UtcDateTimeType>();
            Map(x => x.Status, "Status").CustomType<OrderStatus>();
            Map(x => x.ModifiedUtc, "ModifiedUtc").CustomType<UtcDateTimeType>();

            Component(x => x.Lessor, c =>
            {
                c.Component(y => y.LessorId, d => d.Map(z => z.Id, "Lessor_LessorId"));
                c.Map(y => y.Name, "Lessor_Name");
            });

            Component(x => x.Lessee, c =>
            {
                c.Map(x => x.Id, "Borrower_Id");
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