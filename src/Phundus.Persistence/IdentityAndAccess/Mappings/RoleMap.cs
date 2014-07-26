namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccessCtx.DomainModel;
    using FluentNHibernate.Mapping;

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.Name);
        }
    }
}