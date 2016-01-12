namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using Integration.IdentityAccess;

    public class UserDto : IUser
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public int Version { get; set; }

        public int? JsNummer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EmailAddress { get; set; }
        public bool IsApproved { get; set; }

        public DateTime SignedUpAtUtc { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public bool IsLockedOut { get; set; }

        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}