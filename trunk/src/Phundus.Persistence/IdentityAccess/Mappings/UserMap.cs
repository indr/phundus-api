namespace Phundus.Persistence.IdentityAccess.Mappings
{
    using Common.Domain.Model;
    using FluentNHibernate.Mapping;
    using Phundus.IdentityAccess.Users.Model;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            SchemaAction.Validate();

            Table("Dm_IdentityAccess_User");
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Component(x => x.UserId, a =>
                a.Map(x => x.Id, "Guid"));


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