namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Organizations.Model;
    using FluentNHibernate.Mapping;

    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            SchemaAction.Validate();

            Id(x => x.Id, "Guid").GeneratedBy.Assigned();
            Version(x => x.Version);
            
            Map(x => x.Plan, "[Plan]").CustomType<OrganizationPlan>();
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.Coordinate);
            Map(x => x.Startpage);
            Map(x => x.EmailAddress);
            Map(x => x.Website);
            Map(x => x.CreateDate, "CreateDate").Not.Update();
            Map(x => x.DocTemplateFileName);

            HasMany(x => x.Memberships)
                .KeyColumn("OrganizationGuid")
                .AsSet().Cascade.SaveUpdate();
        }
    }
}