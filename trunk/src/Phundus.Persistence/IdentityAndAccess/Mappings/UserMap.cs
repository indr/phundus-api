namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Users.Model;
    using FluentNHibernate.Mapping;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_User");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Guid, "Guid");
            Version(x => x.Version);

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Street);
            Map(x => x.Postcode);
            Map(x => x.City);
            Map(x => x.MobileNumber);
            Map(x => x.JsNumber);

            HasOne(x => x.Account).Cascade.All();

            Map(x => x.Role, "RoleId").CustomType<UserRole>();
        }
    }
}