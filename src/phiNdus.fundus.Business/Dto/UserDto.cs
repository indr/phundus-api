using System;

namespace phiNdus.fundus.Business.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}