namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Organizations.Model;
    using NHibernate.Type;

    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_Organization");
            Id(x => x.Id, "Guid").GeneratedBy.Assigned();
            Version(x => x.Version);
            
            Map(x => x.Plan, "[Plan]").CustomType<OrganizationPlan>();
            Map(x => x.Name, "Name");
            Map(x => x.Address, "Address");            
            Map(x => x.Startpage, "Startpage");
            Map(x => x.EmailAddress, "EmailAddress");
            Map(x => x.Website, "Website");
            Map(x => x.EstablishedAtUtc, "CreateDate").CustomType<UtcDateTimeType>().Not.Update();
            Map(x => x.DocTemplateFileName, "DocTemplateFileName");

            HasMany(x => x.Memberships)
                .KeyColumn("OrganizationGuid")
                .AsSet().Cascade.SaveUpdate();
        }
    }
}