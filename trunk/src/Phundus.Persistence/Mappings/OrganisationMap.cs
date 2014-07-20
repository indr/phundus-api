namespace Phundus.Persistence.Mappings
{
    using Core.OrganisationCtx.DomainModel;
    using FluentNHibernate.Mapping;

    public class OrganisationMap : ClassMap<Organisation>
    {
        public OrganisationMap()
        {
            Table("Organization");
            Id(x => x.Id);
            Version(x => x.Version);
        }
    }
}