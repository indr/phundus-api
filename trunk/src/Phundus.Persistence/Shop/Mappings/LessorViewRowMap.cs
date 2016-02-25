namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class LessorViewRowMap : ClassMap<LessorViewRow>
    {
        public LessorViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_Shop_Lessors");

            Id(x => x.LessorGuid, "LessorGuid");
            Map(x => x.LessorType, "LessorType");
            Map(x => x.Address, "Address");
            Map(x => x.Name, "Name");
            Map(x => x.EmailAddress, "EmailAddress");
            Map(x => x.PhoneNumber, "PhoneNumber");
            Map(x => x.PublicRental, "PublicRental");
        }
    }
}