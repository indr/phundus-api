namespace Phundus.Persistence.IdentityAccess.Projections
{
    using FluentNHibernate.Mapping;
    using Phundus.IdentityAccess.Projections;

    public class UserAddressDataMap : ClassMap<UserAddressData>
    {
        public UserAddressDataMap()
        {
            SchemaAction.All();
            Table("Es_IdentityAccess_UserAddress");

            Id(x => x.UserId).GeneratedBy.Assigned();
            Map(x => x.UserShortId).Unique();
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Street);
            Map(x => x.Postcode);
            Map(x => x.City);
            Map(x => x.EmailAddress);
            Map(x => x.PhoneNumber);
        }
    }
}