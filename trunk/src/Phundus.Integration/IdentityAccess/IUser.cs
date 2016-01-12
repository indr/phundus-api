namespace Phundus.Integration.IdentityAccess
{
    using System;

    public interface IUser
    {
        int Id { get; }
        Guid Guid { get; }
        int Version { get; }
        int? JsNumber { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        bool IsApproved { get; }
        DateTime CreateDate { get; }
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