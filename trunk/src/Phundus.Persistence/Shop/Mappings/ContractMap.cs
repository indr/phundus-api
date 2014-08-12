namespace Phundus.Persistence.Shop.Mappings
{
    using Core.Shop.Contracts.Model;
    using FluentNHibernate.Mapping;

    public class ContractMap : ClassMap<Contract>
    {
        public ContractMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.CreatedOn, "CreatedOn").Not.Update();
            Map(x => x.OrganizationId, "OrganizationId").Not.Update();
            Map(x => x.SignedOn, "SignedOn");

            Component(x => x.Borrower, c =>
            {
                c.Map(x => x.Id, "Borrower_Id");
                c.Map(x => x.FirstName, "Borrower_FirstName");
                c.Map(x => x.LastName, "Borrower_LastName");
                c.Map(x => x.Street, "Borrower_Street");
                c.Map(x => x.Postcode, "Borrower_Postcode");
                c.Map(x => x.City, "Borrower_City");
                c.Map(x => x.EmailAddress, "Borrower_Email");
                c.Map(x => x.MobilePhoneNumber, "Borrower_MobilePhoneNumber");
                c.Map(x => x.MemberNumber, "Borrower_MemberNumber");
            });
        }
    }
}