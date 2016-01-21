namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Organizations.Model;
    using NHibernate.Linq;
    using NHibernate.Type;

    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Organization");
            CompositeId(x => x.Id).KeyProperty(x => x.Id, "Guid");
            Version(x => x.Version);

            Map(x => x.EstablishedAtUtc, "CreateDate").CustomType<UtcDateTimeType>().Not.Update();
            Map(x => x.Name, "Name");
            Map(x => x.Plan, "[Plan]").CustomType<OrganizationPlan>();

            Component(x => x.ContactDetails, a =>
            {
                a.Map(x => x.PostAddress, "Address");
                a.Map(x => x.PhoneNumber, "PhoneNumber");
                a.Map(x => x.EmailAddress, "EmailAddress");
                a.Map(x => x.Website, "Website");
            });

            Map(x => x.Startpage, "Startpage");
            Map(x => x.DocTemplateFileName, "DocTemplateFileName");

            HasMany(x => x.Memberships)
                .KeyColumn("OrganizationGuid")
                .AsSet().Cascade.SaveUpdate();

            Component(x => x.Settings, a =>
            {
                a.Map(x => x.PublicRental, "Settings_PublicRental");
            });
        }
    }
}