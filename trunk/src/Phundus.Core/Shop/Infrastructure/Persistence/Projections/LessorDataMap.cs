namespace Phundus.Shop.Infrastructure.Persistence.Projections
{
    using Application;
    using FluentNHibernate.Mapping;

    public class LessorDataMap : ClassMap<LessorData>
    {
        public LessorDataMap()
        {
            SchemaAction.All();
            Table("Es_Shop_Lessors");

            Id(x => x.LessorId, "LessorId").GeneratedBy.Assigned();
            Map(x => x.Type, "Type").CustomType<LessorType>();
            Map(x => x.Name, "Name");
            Map(x => x.Url, "Url");
            Map(x => x.PostalAddress, "PostalAddress");
            Map(x => x.PhoneNumber, "PhoneNumber");
            Map(x => x.EmailAddress, "EmailAddress");
            Map(x => x.Website, "Website");
            Map(x => x.PublicRental, "PublicRental");
        }
    }
}