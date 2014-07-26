namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.OrganizationAndMembershipCtx.Model;
    using FluentNHibernate.Mapping;

    public class OrganizationMap : ClassMap<Organization>
    {
        public OrganizationMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.Coordinate);
            Map(x => x.Startpage);
            Map(x => x.EmailAddress);
            Map(x => x.Website);
            Map(x => x.CreateDate).ReadOnly();
            Map(x => x.DocTemplateFileName);

            HasMany(x => x.Memberships)
                .KeyColumn("OrganizationId")
                .AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}