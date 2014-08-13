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

            Map(x => x.CreatedOn, "CreateDate");
            Map(x => x.Status, "Status").CustomType<OrderStatus>();
            Map(x => x.OrganizationId, "OrganizationId");
            
            Component(x => x.Borrower, c =>
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
                //.Access.BackingField()
                .Cascade.AllDeleteOrphan();
        }
    }
}