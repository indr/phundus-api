namespace Phundus.Integration.IdentityAccess
{
    using System;

    public interface IUser
    {
        int UserId { get; }
        Guid UserGuid { get; }

        int RoleId { get; }
        string RoleName { get; }
        
        string EmailAddress { get; }
        string FullName { get; }
        
        string FirstName { get; }
        string LastName { get; }
        string Street { get; }
        string Postcode { get; }
        string City { get; }
        string MobilePhone { get; }
        int? JsNummer { get; }

        bool IsApproved { get; }
        bool IsLockedOut { get; }
        
        DateTime SignedUpAtUtc { get; }
        DateTime? LastLogInAtUtc { get; }
    }
}