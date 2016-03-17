namespace Phundus.IdentityAccess.Infrastructure.Persistence.Projections
{
    using Application;
    using Common.Infrastructure.Persistence;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class OrganizationDataMap : ClassMap<OrganizationData>
    {
        public OrganizationDataMap()
        {
            SchemaAction.All();
            Table("Es_IdentityAccess_Organizations");

            Id(x => x.OrganizationId).GeneratedBy.Assigned();
            Map(x => x.EstablishedAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.Name);
            Map(x => x.Url);
            Map(x => x.Plan, "[Plan]");
            Map(x => x.PublicRental);

            Map(x => x.Line1);
            Map(x => x.Line2);
            Map(x => x.Street);
            Map(x => x.Postcode);
            Map(x => x.City);
            Map(x => x.PostalAddress).WithMaxSize();

            Map(x => x.EmailAddress);
            Map(x => x.PhoneNumber);
            Map(x => x.Website);

            Map(x => x.Startpage).WithMaxSize();
        }
    }
}