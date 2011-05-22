using System;

namespace phiNdus.fundus.Core.Business.Dto
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
    }
}