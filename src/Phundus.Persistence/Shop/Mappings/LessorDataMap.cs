namespace Phundus.Persistence.Shop.Mappings
{
    using FluentNHibernate.Mapping;
    using Phundus.Shop.Projections;

    public class LessorDataMap : ClassMap<LessorData>
    {
        public LessorDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Lessors");

            Id(x => x.LessorId, "LessorId").GeneratedBy.Assigned();
            Map(x => x.Type, "LessorType").CustomType<LessorData.LessorType>();
            Map(x => x.Name, "Name");
            Map(x => x.PostalAddress, "PostalAddress");
            Map(x => x.PhoneNumber, "PhoneNumber");
            Map(x => x.EmailAddress, "EmailAddress");            
            Map(x => x.PublicRental, "PublicRental");
        }
    }
}