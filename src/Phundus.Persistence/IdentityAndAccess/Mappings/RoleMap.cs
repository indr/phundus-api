namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Users.Model;
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