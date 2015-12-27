namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;

    public class UserDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int Version { get; set; }

        public int? JsNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public bool IsApproved { get; set; }

        public DateTime CreateDate { get; set; }
        
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public bool IsLockedOut { get; set; }

        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string MobilePhone { get; set; }
        
    }
}