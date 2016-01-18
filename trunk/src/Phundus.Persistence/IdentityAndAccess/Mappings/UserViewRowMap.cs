namespace Phundus.Persistence.IdentityAndAccess.Mappings
{
    using FluentNHibernate.Mapping;
    using IdentityAccess.Queries.QueryModels;
    using NHibernate.Type;

    public class UserViewRowMap : ClassMap<UserViewRow>
    {
        public UserViewRowMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("View_IdentityAccess_Users");

            Id(x => x.UserId, "UserGuid");
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
            Map(x => x.SignedUpAtUtc, "SignedUpAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.LastLogInAtUtc, "LastLogInAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.LastPasswordChangeAtUtc, "LastPasswordChangeAtUtc").CustomType<UtcDateTimeType>();
            Map(x => x.LastLockOutAtUtc, "LastLockOutAtUtc").CustomType<UtcDateTimeType>();
        }
    }
}