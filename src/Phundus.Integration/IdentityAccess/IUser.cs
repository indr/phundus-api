namespace Phundus.Integration.IdentityAccess
{
    using System;

    public interface IUser
    {
        int UserId { get; }
        Guid UserGuid { get; }
       
        int? JsNummer { get; }
        string FirstName { get; }
        string LastName { get; }
        string EmailAddress { get; }
        bool IsApproved { get; }
        DateTime SignedUpAtUtc { get; }
        int RoleId { get; }
        string RoleName { get; }
        bool IsLockedOut { get; }
        string Street { get; }
        string Postcode { get; }
        string City { get; }
        string MobilePhone { get; }
        string FullName { get; }
    }
}