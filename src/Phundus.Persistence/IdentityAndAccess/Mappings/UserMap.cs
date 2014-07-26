namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using Core.IdentityAndAccess.Users.Model;
    using FluentNHibernate.Mapping;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Version(x => x.Version);

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Street);
            Map(x => x.Postcode);
            Map(x => x.City);
            Map(x => x.MobileNumber);
            Map(x => x.JsNumber);

            HasOne(x => x.Account).Cascade.All();
            References(x => x.Role).Column("RoleId").Cascade.None().LazyLoad(Laziness.False);

            /*
            <set name="Memberships" cascade="all-delete-orphan" lazy="true" inverse="true" >
                <key column="UserId" />
                <one-to-many class="Phundus.Core.Entities.OrganizationMembership" />
            </set>
             
            <many-to-one name="SelectedOrganization" column="SelectedOrganizationId" /> 
             
            */
        }
    }
}