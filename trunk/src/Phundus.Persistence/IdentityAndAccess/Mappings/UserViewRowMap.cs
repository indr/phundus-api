namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Queries.QueryModels;

    public class UserViewRowMap : ClassMap<UserViewRow>
    {
        public UserViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_IdentityAccess_Users");

            Id(x => x.UserGuid, "UserGuid");
            Map(x => x.RoleId, "RoleId");
            Map(x => x.EmailAddress, "EmailAddress");
            Map(x => x.FirstName, "FirstName");
            Map(x => x.LastName, "LastName");
            Map(x => x.Street, "Street");
            Map(x => x.Postcode, "Postcode");
            Map(x => x.City, "City");
            Map(x => x.MobilePhone, "PhoneNumber");
            Map(x => x.JsNummer, "JsNummer");
            Map(x => x.IsApproved, "IsApproved");
            Map(x => x.IsLockedOut, "IsLockedOut");
            Map(x => x.SignedUpAtUtc, "SignedUpAtUtc");
            Map(x => x.LastLogInAtUtc, "LastLogInAtUtc");
            Map(x => x.LastPasswordChangeAtUtc, "LastPasswordChangeAtUtc");
            Map(x => x.LastLockOutAtUtc, "LastLockOutAtUtc");
        }
    }
}